using Rybu4WS.Language;
using Rybu4WS.StateMachine.Composed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.StateMachine
{
    public class Converter
    {
        private ListStatePairEqualityComparer _listStatePairEqualityComparer = new ListStatePairEqualityComparer();

        private static readonly string InitCallerName = "INIT";

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

            foreach (var group in system.Groups)
            {
                result.ComposedGraphs.Add(Convert(group));
            }

            return result;
        }

        public Graph Convert(Language.Process process, string sendMessageServerName = null, List<StatePair> initState = null)
        {
            var graph = new Graph() { Name = process.ServerName, AgentIndex = process.AgentIndex };

            sendMessageServerName ??= process.ServerName;

            initState = new List<StatePair>(initState ?? new List<StatePair>());
            graph.InitNode = graph.GetOrCreateIdleNode(initState);

            var unhandledMessages = HandleCode(graph, graph.InitNode, process.Statements, InitCallerName, sendMessageServerName, $"START_FROM_{InitCallerName}");
            foreach (var item in unhandledMessages)
            {
                graph.CreateEdge(item.Source, item.Source, item.Message, (sendMessageServerName, $"MISSING_CODE_AFTER_{item.Source.CodeLocation}"));
            }

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
                var preStates = GetCartesianStates(server, branch.Condition);

                foreach (var states in preStates)
                {
                    var beforeCallNode = graph.GetOrCreateIdleNode(states);
                    var unhandledMessages = HandleCode(graph, beforeCallNode, branch.Statements, caller, server.Name, $"CALL_{action.Name}_FROM_{caller}");
                    foreach (var item in unhandledMessages)
                    {
                        graph.CreateEdge(item.Source, item.Source, item.Message, (server.Name, $"MISSING_CODE_AFTER_{item.Source.CodeLocation}"));
                    }
                }
            }   
        }

        private struct PendingMessage
        {
            public Node Source;

            public string Message;

            public static PendingMessage Create(Node source, string message)
            {
                return new PendingMessage() { Source = source, Message = message };
            }

            public static PendingMessage FromEdge(Edge edge)
            {
                return new PendingMessage() { Source = edge.Target, Message = edge.SendMessage };
            }
        }

        private IEnumerable<PendingMessage> HandleCode(Graph graph, Node currentNode, List<BaseStatement> statements, string caller, string serverName, string receiveMessage)
        {
            var currentStatementIndex = 0;
            var currentStatement = statements[currentStatementIndex];

            var firstNode = graph.GetOrCreateNode(currentNode.States, caller, currentStatement.CodeLocation);
            var toHandle = new List<PendingMessage>();
            var newToHandle = new List<PendingMessage>();
            var firstEdge = graph.CreateEdge(
                currentNode,
                firstNode,
                receiveMessage,
                (serverName, $"EXEC_{firstNode.CodeLocation}_FROM_{caller}"));
            newToHandle.Add(PendingMessage.FromEdge(firstEdge));

            while (currentStatementIndex < statements.Count)
            {
                currentStatement = statements[currentStatementIndex];
                var nextStatement = (currentStatementIndex + 1 < statements.Count) ? statements[currentStatementIndex + 1] : null;
                toHandle.Clear();
                toHandle.AddRange(newToHandle);
                newToHandle.Clear();

                if (currentStatement is StatementStateMutation currentStatementMutation)
                {
                    if (nextStatement != null)
                    {
                        foreach (var item in toHandle)
                        {
                            var newStates = Mutate(item.Source.States, currentStatementMutation);
                            var mutatedNode = graph.GetOrCreateNode(newStates, caller, nextStatement.CodeLocation);

                            var mutateEdge = graph.CreateEdge(item.Source, mutatedNode, item.Message,
                                (serverName, $"EXEC_{mutatedNode.CodeLocation}_FROM_{caller}"));
                            mutateEdge.StatementReference = currentStatement;
                            newToHandle.Add(PendingMessage.FromEdge(mutateEdge));
                        }
                    }
                    else
                    {
                        foreach (var item in toHandle)
                        {
                            var newStates = Mutate(item.Source.States, currentStatementMutation);
                            var mutatedNodeAfter = graph.GetOrCreateNode(newStates, caller, currentStatement.CodeLocation, true);

                            var mutateEdge = graph.CreateEdge(item.Source, mutatedNodeAfter, item.Message,
                                (serverName, $"PROCEED_{mutatedNodeAfter.CodeLocation}_FROM_{caller}"));
                            mutateEdge.StatementReference = currentStatement;
                            newToHandle.Add(PendingMessage.FromEdge(mutateEdge));
                        }
                        return newToHandle;
                    }
                }
                else if (currentStatement is StatementReturn currentStatementReturn)
                {
                    foreach (var item in toHandle)
                    {
                        var idleNode = graph.GetOrCreateIdleNode(item.Source.States);

                        var returnEdge = graph.CreateEdge(item.Source, idleNode, item.Message,
                            (caller, $"RETURN_{currentStatementReturn.Value}"));
                        newToHandle.Add(PendingMessage.FromEdge(returnEdge));
                    }
                    
                    return Enumerable.Empty<PendingMessage>(); // end of execution
                }
                else if (currentStatement is StatementCall currentStatementCall)
                {
                    foreach (var item in toHandle)
                    {
                        var nodeAtCall = graph.GetOrCreateNode(item.Source.States, caller, currentStatement.CodeLocation, true);

                        graph.CreateEdge(item.Source, nodeAtCall, item.Message,
                            (currentStatementCall.ServerName, $"CALL_{currentStatementCall.ActionName}_FROM_{serverName}"));

                        if (currentStatementCall.ServerActionReference.CanTerminate)
                        {
                            HandleTerminate(graph, nodeAtCall, serverName, caller);
                        }

                        if (nextStatement != null)
                        {
                            var nextStatementNode = graph.GetOrCreateNode(item.Source.States, caller, nextStatement.CodeLocation);
                            foreach (var possibleReturn in currentStatementCall.ServerActionReference.PossibleReturnValues)
                            {
                                var returnEdge = graph.CreateEdge(nodeAtCall, nextStatementNode,
                                    $"RETURN_{possibleReturn}",
                                    (serverName, $"EXEC_{nextStatementNode.CodeLocation}_FROM_{caller}"));
                                newToHandle.Add(PendingMessage.FromEdge(returnEdge));
                            }
                        }
                        else
                        {
                            foreach (var possibleReturn in currentStatementCall.ServerActionReference.PossibleReturnValues)
                            {
                                newToHandle.Add(PendingMessage.Create(nodeAtCall, $"RETURN_{possibleReturn}"));
                            }
                        }
                    }

                    if (nextStatement == null) return newToHandle;
                }
                else if (currentStatement is StatementMatch currentStatementMatch)
                {
                    foreach (var item in toHandle)
                    {
                        var nodeAtCall = graph.GetOrCreateNode(item.Source.States, caller, currentStatement.CodeLocation, true);

                        graph.CreateEdge(item.Source, nodeAtCall, item.Message,
                            (currentStatementMatch.ServerName, $"CALL_{currentStatementMatch.ActionName}_FROM_{serverName}"));

                        if (currentStatementMatch.ServerActionReference.CanTerminate)
                        {
                            HandleTerminate(graph, nodeAtCall, serverName, caller);
                        }

                        var pendingToHandle = new List<PendingMessage>();
                        foreach (var handler in currentStatementMatch.Handlers)
                        {
                            var nodesToHandle = HandleCode(graph, nodeAtCall, handler.HandlerStatements, caller, serverName,
                                $"RETURN_{handler.HandledValue}");

                            pendingToHandle.AddRange(nodesToHandle);
                        }

                        var unhandledReturnValues = currentStatementMatch.ServerActionReference.PossibleReturnValues.Except(currentStatementMatch.Handlers.Select(x => x.HandledValue));
                        if (nextStatement != null)
                        {
                            var nextStatementNodeUnhandledReturn = graph.GetOrCreateNode(item.Source.States, caller, nextStatement.CodeLocation);
                            foreach (var possibleReturn in unhandledReturnValues)
                            {
                                var returnEdge = graph.CreateEdge(nodeAtCall, nextStatementNodeUnhandledReturn,
                                    $"RETURN_{possibleReturn}",
                                    (serverName, $"EXEC_{nextStatementNodeUnhandledReturn.CodeLocation}_FROM_{caller}"));
                                newToHandle.Add(PendingMessage.FromEdge(returnEdge));
                            }
                            foreach (var itemFromHandler in pendingToHandle)
                            {
                                var nextStatementNode = graph.GetOrCreateNode(itemFromHandler.Source.States, caller, nextStatement.CodeLocation);
                                var handlerEdge = graph.CreateEdge(itemFromHandler.Source, nextStatementNode,
                                    itemFromHandler.Message,
                                    (serverName, $"EXEC_{nextStatementNode.CodeLocation}_FROM_{caller}"));
                                newToHandle.Add(PendingMessage.FromEdge(handlerEdge));
                            }
                        }
                        else
                        {
                            newToHandle.AddRange(pendingToHandle);
                            foreach (var possibleReturn in unhandledReturnValues)
                            {
                                newToHandle.Add(PendingMessage.Create(nodeAtCall, $"RETURN_{possibleReturn}"));
                            }
                        }
                    }

                    if (nextStatement == null) return newToHandle;
                }
                else if (currentStatement is StatementTerminate)
                {
                    foreach (var item in toHandle)
                    {
                        HandleTerminate(graph, item.Source, serverName, caller, item.Message);
                    }

                    return Enumerable.Empty<PendingMessage>(); // end of execution
                    
                }
                else if (currentStatement is StatementLoop currentStatementLoop)
                {
                    List<PendingMessage> handledPendingMessages = new List<PendingMessage>();
                    while (toHandle.Count > 0)
                    {
                        foreach (var item in toHandle)
                        {
                            var loopNode = graph.GetOrCreateNode(item.Source.States, caller, currentStatement.CodeLocation);
                            graph.CreateEdge(item.Source, loopNode, item.Message,
                                (serverName, $"EXEC_{loopNode.CodeLocation}_FROM_{caller}"));

                            var nodesToHandle = HandleCode(graph, loopNode, currentStatementLoop.LoopStatements, caller, serverName, $"EXEC_{loopNode.CodeLocation}_FROM_{caller}");
                            newToHandle.AddRange(nodesToHandle);
                        }
                        toHandle.Clear();
                        toHandle.AddRange(newToHandle);
                        newToHandle.Clear();

                        // remove already handled loop entrances
                        toHandle.RemoveAll(x => handledPendingMessages.Any(y => x.Message == y.Message && x.Source == y.Source));
                    }
                    return Enumerable.Empty<PendingMessage>(); // end of execution, cannot break from loop
                }
                else
                {
                    throw new NotImplementedException();
                }

                currentStatementIndex++;
            }

            return Enumerable.Empty<PendingMessage>();
        }

        private void HandleTerminate(Graph graph, Node node, string serverName, string caller, string receiveMessage = "TERMINATE")
        {
            var idleNode = graph.GetOrCreateIdleNode(node.States);
            if (caller != InitCallerName)
            {
                graph.CreateEdge(node, idleNode, receiveMessage, (caller, $"TERMINATE"));
            }
            else
            {
                graph.CreateEdge(node, idleNode, receiveMessage, (serverName, $"TERMINATE_EXIT"));
                graph.CreateEdge(idleNode, idleNode, $"TERMINATE_EXIT");
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
                        throw new Exception($"Integer variable '{varState.VariableReference.Name}' out of range! Cannot '{varState.VariableReference.Name} = {mutation.Value}'");
                    }
                    varState.Value = mutation.Value;
                }
                else if (mutation.Operator == StateMutationOperator.Increment)
                {
                    var newValue = intValue + mutationIntValue;
                    if (!varState.VariableReference.AvailableValues.Contains(newValue.ToString()))
                    {
                        throw new Exception($"Integer variable '{varState.VariableReference.Name}' out of range! Cannot '{varState.VariableReference.Name} += {mutationIntValue}' when '{varState.VariableReference.Name} == {intValue}'");
                    }
                    varState.Value = newValue.ToString();
                }
                else if (mutation.Operator == StateMutationOperator.Decrement)
                {
                    var newValue = intValue - mutationIntValue;
                    if (!varState.VariableReference.AvailableValues.Contains(newValue.ToString()))
                    {
                        throw new Exception($"Integer variable '{varState.VariableReference.Name}' out of range! Cannot '{varState.VariableReference.Name} -= {mutationIntValue}' when '{varState.VariableReference.Name} == {intValue}'");
                    }
                    varState.Value = newValue.ToString();
                }
                else if (mutation.Operator == StateMutationOperator.Modulo)
                {
                    var newValue = intValue % mutationIntValue;
                    if (!varState.VariableReference.AvailableValues.Contains(newValue.ToString()))
                    {
                        throw new Exception($"Integer variable '{varState.VariableReference.Name}' out of range! Cannot '{varState.VariableReference.Name} %= {mutationIntValue}' when '{varState.VariableReference.Name} == {intValue}'");
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

        public List<List<StatePair>> GetCartesianStates(Server server, ICondition condition = null)
        {
            if (condition == null) return GetCartesianStatesLeaf(server, null);
            else if (condition is ConditionNode) return GetCartesianStates(server, condition as ConditionNode);
            else if (condition is ConditionLeaf) return GetCartesianStatesLeaf(server, condition as ConditionLeaf);

            throw new Exception("Unsupported condition type");
        }

        public List<List<StatePair>> GetCartesianStates(Server server, ConditionNode conditionNode)
        {
            var leftStates = GetCartesianStates(server, conditionNode.Left);
            var rightStates = GetCartesianStates(server, conditionNode.Right);

            if (conditionNode.Operator == ConditionLogicalOperator.And) return leftStates.Intersect(rightStates, _listStatePairEqualityComparer).ToList();
            else if (conditionNode.Operator == ConditionLogicalOperator.Or) return leftStates.Union(rightStates, _listStatePairEqualityComparer).Distinct(_listStatePairEqualityComparer).ToList();

            throw new Exception("Unsupported condition logic operator");
        }

        public List<List<StatePair>> GetCartesianStatesLeaf(Server server, ConditionLeaf condition)
        {
            var states = new List<List<StatePair>>() { new List<StatePair>() };

            foreach (var variable in server.Variables)
            {
                var newStates = new List<List<StatePair>>();
                foreach (var state in states)
                {
                    foreach (var value in variable.AvailableValues)
                    {
                        bool conditionSatisfied = true;
                        if (condition != null && condition.VariableName == variable.Name)
                        {
                            if (condition.VariableType != variable.Type) throw new Exception("Incorrect condition variable type");
                            conditionSatisfied = CheckCondition(condition, value);
                        }

                        if (conditionSatisfied)
                        {
                            var compose = new List<StatePair>(state);
                            compose.Add(new StatePair(variable.Name, value, variable));
                            newStates.Add(compose);
                        }
                    }
                }
                states = newStates;
            }

            return states;
        }

        private bool CheckCondition(ConditionLeaf condition, string variableValue)
        {
            if (condition == null) return true;

            if (condition.VariableType == VariableType.Integer)
            {
                var intVariableValue = int.Parse(variableValue);
                var conditionIntValue = int.Parse(condition.Value);

                switch (condition.Operator)
                {
                    case ConditionOperator.Equal: return intVariableValue == conditionIntValue;
                    case ConditionOperator.NotEqual: return intVariableValue != conditionIntValue;
                    case ConditionOperator.GreaterThan: return intVariableValue > conditionIntValue;
                    case ConditionOperator.GreaterOrEqualThan: return intVariableValue >= conditionIntValue;
                    case ConditionOperator.LessThan: return intVariableValue < conditionIntValue;
                    case ConditionOperator.LessOrEqualThan: return intVariableValue <= conditionIntValue;
                    default: throw new Exception("Operator not supported");
                }
            }
            else if (condition.VariableType == VariableType.Enum)
            {
                switch (condition.Operator)
                {
                    case ConditionOperator.Equal: return variableValue == condition.Value;
                    case ConditionOperator.NotEqual: return variableValue != condition.Value;
                    default: throw new Exception("Operator not supported");
                }
            }
            else
            {
                throw new Exception("Unknown VariableType");
            }
        }

        public ComposedGraph Convert(Language.Group group)
        {
            var result = new ComposedGraph() { Name = group.ServerName, RequiredAgents = group.Processes.Select(x => x.AgentIndex).ToArray() };
            var initState = new List<StatePair>();
            foreach (var item in group.Variables)
            {
                initState.Add(new StatePair(item));
            }

            var processesGraphs = group.Processes.Select(x => Convert(x, group.ServerName, initState)).ToList();

            Compose(processesGraphs, initState, result);

            return result;
        }

        private void Compose(List<Graph> graphs, List<StatePair> initState, ComposedGraph result)
        {
            result.InitNode = new ComposedNode() { States = new List<StatePair>(initState) };
            foreach (var graph in graphs)
            {
                var agentState = new ComposedNode.AgentState() { BaseNodeReference = graph.InitNode };
                result.InitNode.Agents.Add(graph.AgentIndex, agentState);
            }
            result.Nodes.Add(result.InitNode);
            

            var toProcess = new Stack<ComposedNode>();
            toProcess.Push(result.InitNode);
            while (toProcess.Count > 0)
            {
                var node = toProcess.Pop();
                foreach (var (agentIndex, agent) in node.Agents)
                {
                    foreach (var baseEdge in agent.BaseNodeReference.OutEdges)
                    {
                        var nextBaseNodes = GetBaseNodesWithAgents(node);
                        nextBaseNodes[agentIndex] = baseEdge.Target;

                        var nextStates = node.States;
                        if (baseEdge.StatementReference is StatementStateMutation statementStateMutation)
                        {
                            nextStates = Mutate(node.States, statementStateMutation);
                        }

                        var nextNode = result.GetOrCreateNode(nextBaseNodes, nextStates, out var isNew);
                        var edge = new ComposedEdge()
                        {
                            AgentIndex = agentIndex,
                            Source = node,
                            Target = nextNode,
                            ReceiveMessage = baseEdge.ReceiveMessage,
                            SendMessage = baseEdge.SendMessage,
                            SendMessageServer = baseEdge.SendMessageServer
                        };
                        result.Edges.Add(edge);
                        node.Agents[agentIndex].OutEdges.Add(edge);
                        if (isNew)
                        {
                            toProcess.Push(nextNode);
                        }
                    }
                }
            }
        }

        private Dictionary<int, Node> GetBaseNodesWithAgents(ComposedNode composedNode)
        {
            return composedNode.Agents.ToDictionary(x => x.Key, x => x.Value.BaseNodeReference);
        }

        public IEnumerable<List<Node>> GetNodeCombinations(Graph[] graphs)
        {
            var currentGraph = graphs[0];

            if (graphs.Length == 1)
            {
                foreach (var node in currentGraph.Nodes)
                {
                    yield return new List<Node>() { node };
                }
                yield break;
            }

            var restGraphs = graphs.Skip(1).ToArray();
            var restCombinations = GetNodeCombinations(restGraphs).ToList();

            foreach (var node in currentGraph.Nodes)
            {
                foreach (var combs in restCombinations)
                {
                    var result = new List<Node>();
                    result.Add(node);
                    result.AddRange(combs);
                    yield return result;
                }
            }
        }
    }
}
