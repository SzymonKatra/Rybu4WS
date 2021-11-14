﻿using Rybu4WS.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.StateMachine
{
    public class Converter
    {
        public StateMachineSystem Convert(Language.System system)
        {
            var result = new StateMachineSystem();
            result.SystemReference = system;

            foreach (var server in system.Servers)
            {
                result.Graphs.Add(Convert(server));
            }

            foreach (var process in system.Processes)
            {
                result.Graphs.Add(Convert(process));
            }

            return result;
        }

        public Graph Convert(Language.Process process)
        {
            var graph = new Graph() { Name = process.Name };

            var initState = new List<StatePair>();
            graph.InitNode = graph.GetOrCreateIdleNode(initState);

            HandleCode(graph, graph.InitNode, process.Statements, "INIT", process.Name, $"START_FROM_INIT");

            return graph;
        }

        public Graph Convert(Language.Server server)
        {
            var graph = new Graph() { Name = server.Name };

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

        private void HandleServerActionBranch(Graph graph, ServerActionBranch branch, ServerAction action, Language.Server server)
        {
            foreach (var caller in action.Callers)
            {
                var preStates = server.GetCartesianStates(branch.Condition);

                foreach (var states in preStates)
                {
                    var beforeCallNode = graph.GetOrCreateIdleNode(states);
                    HandleCode(graph, beforeCallNode, branch.Statements, caller, server.Name, $"{action.Name}_FROM_{caller}");
                }
            }   
        }

        private void HandleCode(Graph graph, Node currentNode, List<BaseStatement> statements, string caller, string serverName, string receiveMessage, Node nextNodeWhenEndOfCode = null)
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
                receiveMessage,
                (serverName, $"ENTER_PRE_{nextNode.CodeLocation}_FROM_{caller}"));

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
                        graph.Nodes.Add(nextNode);
                        lastEdge = graph.CreateEdge(currentNode, nextNode, lastEdge.SendMessage,
                            (serverName, $"ENTER_PRE_{nextNode.CodeLocation}_FROM_{caller}"));
                    }
                    else
                    {
                        if (nextNodeWhenEndOfCode == null)
                        {
                            lastEdge = graph.CreateEdge(currentNode, currentNode, lastEdge.SendMessage,
                                (serverName, $"MISSING_CODE_AFTER_{currentNode.CodeLocation}_FROM_{caller}"));
                            break;
                        }
                        else
                        {
                            lastEdge = graph.CreateEdge(currentNode, nextNodeWhenEndOfCode, lastEdge.SendMessage,
                                (serverName, $"ENTER_PRE_{nextNodeWhenEndOfCode.CodeLocation}_FROM_{caller}"));
                            break;
                        }
                    }
                }
                else if (currentStatement is StatementReturn currentStatementReturn)
                {
                    nextNode = graph.GetOrCreateIdleNode(currentNode.States);
                    lastEdge = graph.CreateEdge(currentNode, nextNode, lastEdge.SendMessage,
                        (caller, $"RETURN_{currentStatementReturn.Value}"));

                    break; // end of execution
                }
                else if (currentStatement is StatementCall currentStatementCall)
                {
                    nextNode = new Node()
                    {
                        States = currentNode.States,
                        Caller = currentNode.Caller,
                        CodeLocation = currentStatement.CodeLocation,
                        IsPending = true
                    };
                    graph.Nodes.Add(nextNode);
                    lastEdge = graph.CreateEdge(currentNode, nextNode, lastEdge.SendMessage,
                        (currentStatementCall.ServerName, $"{currentStatementCall.ActionName}_FROM_{serverName}"));

                    currentNode = nextNode;

                    if (nextStatement != null)
                    {
                        nextNode = new Node()
                        {
                            States = currentNode.States,
                            Caller = currentNode.Caller,
                            CodeLocation = nextStatement.CodeLocation
                        };
                        graph.Nodes.Add(nextNode);
                        
                    }
                    else
                    {
                        nextNode = nextNodeWhenEndOfCode;
                    }

                    foreach (var possibleReturn in currentStatementCall.ServerActionReference.PossibleReturnValues)
                    {
                        if (nextNode != null)
                        {
                            lastEdge = graph.CreateEdge(currentNode, nextNode,
                                $"RETURN_{possibleReturn}",
                                (serverName, $"ENTER_PRE_{nextNode.CodeLocation}_FROM_{caller}"));
                        }
                        else
                        {
                            lastEdge = graph.CreateEdge(currentNode, currentNode,
                                $"RETURN_{possibleReturn}",
                                (serverName, $"MISSING_CODE_AFTER_{currentNode.CodeLocation}_FROM_{caller}"));
                        }
                    }

                    if (nextStatement == null) break;
                }
                else if (currentStatement is StatementMatch currentStatementMatch)
                {
                    nextNode = new Node()
                    {
                        States = currentNode.States,
                        Caller = currentNode.Caller,
                        CodeLocation = currentStatement.CodeLocation,
                        IsPending = true
                    };
                    graph.Nodes.Add(nextNode);
                    lastEdge = graph.CreateEdge(currentNode, nextNode, lastEdge.SendMessage,
                        (currentStatementMatch.ServerName, $"{currentStatementMatch.ActionName}_FROM_{serverName}"));

                    currentNode = nextNode;

                    if (nextStatement != null)
                    {
                        nextNode = new Node()
                        {
                            States = currentNode.States,
                            Caller = currentNode.Caller,
                            CodeLocation = nextStatement.CodeLocation
                        };
                        graph.Nodes.Add(nextNode);
                    }   
                    else
                    {
                        nextNode = nextNodeWhenEndOfCode;
                    }

                    foreach (var handler in currentStatementMatch.Handlers)
                    {
                        HandleCode(graph, currentNode, handler.HandlerStatements, caller, serverName,
                            $"RETURN_{handler.HandledValue}", nextNode);
                    }
                    var handledReturnValues = currentStatementMatch.Handlers.Select(x => x.HandledValue);

                    foreach (var possibleReturn in currentStatementMatch.ServerActionReference.PossibleReturnValues.Except(handledReturnValues))
                    {
                        if (nextNode != null)
                        {
                            lastEdge = graph.CreateEdge(currentNode, nextNode,
                                $"RETURN_{possibleReturn}",
                                (serverName, $"ENTER_PRE_{nextNode.CodeLocation}_FROM_{caller}"));
                        }
                        else
                        {
                            lastEdge = graph.CreateEdge(currentNode, currentNode,
                                $"RETURN_{possibleReturn}",
                                (serverName, $"MISSING_CODE_AFTER_{currentNode.CodeLocation}_FROM_{caller}"));
                        }
                    }

                    if (nextStatement == null) break;
                }
                else if (currentStatement is StatementTerminate)
                {
                    nextNode = graph.GetOrCreateIdleNode(currentNode.States);
                    lastEdge = graph.CreateEdge(currentNode, nextNode, lastEdge.SendMessage);
                    break;
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