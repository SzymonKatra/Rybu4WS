using Rybu4WS.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.StateMachine
{
    public class Converter
    {
        public Graph Convert(Logic.Server server)
        {
            var graph = new Graph();

            var initState = new List<StatePair>();
            foreach (var item in server.Variables)
            {
                initState.Add(new StatePair(item));
            }

            graph.InitNode = graph.GetOrCreateIdleNode(initState);

            foreach (var action in server.Actions)
            {
                foreach (var branch in action.Branches)
                {
                    HandleServerActionBranch(graph, branch, action, server);
                }
            }

            return graph;
        }

        private void HandleServerActionBranch(Graph graph, ServerActionBranch branch, ServerAction action, Logic.Server server)
        {
            foreach (var caller in action.Callers)
            {
                var preStates = server.GetCartesianStates(branch.Condition);

                foreach (var states in preStates)
                {
                    var beforeCallNode = graph.GetOrCreateIdleNode(states);
                    HandleCode(graph, beforeCallNode, branch.Statements, action, caller, server);
                }
            }   
        }

        private void HandleCode(Graph graph, Node currentNode, List<BaseStatement> statements, ServerAction action, string caller, Logic.Server server)
        {
            var currentStatementIndex = 0;
            var currentStatement = statements[currentStatementIndex];

            var nextNode = new Node()
            {
                States = new List<StatePair>(currentNode.States),
                Caller = caller,
                CodeLocation = currentStatement.CodeLocation
            };
            graph.Nodes.Add(nextNode);
            var lastEdge = graph.CreateEdge(
                currentNode,
                nextNode,
                $"{server.Name}.{action.Name}_FROM_{caller}",
                $"{server.Name}.ENTER_PRE_{nextNode.CodeLocation}_FROM_{caller}");

            currentNode = nextNode;

            while (currentStatementIndex < statements.Count)
            {
                currentStatement = statements[currentStatementIndex];
                var nextStatement = (currentStatementIndex + 1 < statements.Count) ? statements[currentStatementIndex + 1] : null;

                if (currentStatement is StatementStateMutation currentStatementMutation)
                {
                    var newStates = Mutate(currentNode.States, currentStatementMutation);
                    if (nextStatement != null)
                    {
                        nextNode = new Node()
                        {
                            States = newStates,
                            Caller = currentNode.Caller,
                            CodeLocation = nextStatement.CodeLocation,
                        };
                        lastEdge = graph.CreateEdge(currentNode, nextNode, lastEdge.SendMessage,
                            $"{server.Name}.ENTER_PRE_{nextNode.CodeLocation}_FROM_{caller}");
                    }
                    else
                    {
                        // no more steps, probably error, will not return anywhere
                        nextNode = graph.GetOrCreateIdleNode(newStates);
                        graph.CreateEdge(currentNode, nextNode, lastEdge.SendMessage, $"{server.Name}.ENTER_AFTER_{currentStatement.CodeLocation}_FROM_{caller}");
                    }
                }
                else if (currentStatement is StatementReturn currentStatementReturn)
                {
                    nextNode = graph.GetOrCreateIdleNode(currentNode.States);
                    lastEdge = graph.CreateEdge(currentNode, nextNode, lastEdge.SendMessage,
                        $"{caller}.RETURN_{currentStatementReturn.Value}");

                    break; // end of execution
                }
                else
                {
                    throw new NotImplementedException();
                }

                currentNode = nextNode;
                currentStatementIndex++;
            }
        }

        public List<StatePair> Mutate(List<StatePair> states, StatementStateMutation mutation)
        {
            var copiedStates = new List<StatePair>(states);

            var varStateIndex = copiedStates.FindIndex(x => x.Name == mutation.VariableName);
            if (varStateIndex == -1) throw new Exception($"Cannot find variable {mutation.VariableName} in states");
            var varState = copiedStates[varStateIndex];

            if (varState.VariableReference.Type == VariableType.Enum)
            {
                if (mutation.Operator != StateMutationOperator.Assignment)
                {
                    throw new Exception("Only assignment operator is available for enums");
                }
                if (!varState.VariableReference.AvailableValues.Contains(mutation.Value))
                {
                    throw new Exception($"Enum variable '{varState.VariableReference.Name}' does not support value '{mutation.Value}'");
                }

                varState.Value = mutation.Value;
            }
            else if (varState.VariableReference.Type == VariableType.Integer)
            {
                int intValue = int.Parse(varState.Value);
                int mutationIntValue = int.Parse(mutation.Value);

                if (mutation.Operator == StateMutationOperator.Assignment)
                {
                    if (!varState.VariableReference.AvailableValues.Contains(mutation.Value))
                    {
                        throw new Exception($"Integer variable '{varState.VariableReference.Name}' does not support value '{mutation.Value}'");
                    }
                    varState.Value = mutation.Value;
                }
                else if (mutation.Operator == StateMutationOperator.Increment)
                {
                    var newValue = intValue + mutationIntValue;
                    if (!varState.VariableReference.AvailableValues.Contains(newValue.ToString()))
                    {
                        throw new Exception($"Integer variable '{varState.VariableReference.Name}' does not support value '{newValue}' ({intValue} + {mutationIntValue})");
                    }
                    varState.Value = newValue.ToString();
                }
                else if (mutation.Operator == StateMutationOperator.Decrement)
                {
                    var newValue = intValue - mutationIntValue;
                    if (!varState.VariableReference.AvailableValues.Contains(newValue.ToString()))
                    {
                        throw new Exception($"Integer variable '{varState.VariableReference.Name}' does not support value '{newValue}' ({intValue} - {mutationIntValue})");
                    }
                    varState.Value = newValue.ToString();
                }
                else
                {
                    throw new NotSupportedException("Mutation operator not supported");
                }
            }
            else
            {
                throw new NotSupportedException("Variable type not supported");
            }

            copiedStates[varStateIndex] = varState;

            return copiedStates;
        }
    }
}
