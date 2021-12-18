using Rybu4WS.Language;
using Rybu4WS.StateMachine.Composed;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.StateMachine
{
    public class Converter
    {
        private ListStatePairEqualityComparer _listStatePairEqualityComparer = new ListStatePairEqualityComparer();

        private static readonly string InitCallerName = "INIT";

        private TextWriter _errorTextWriter;

        private class AbortConvertException : Exception
        {
            public CodeLocation? CodeLocation { get; set; }

            public AbortConvertException(string message, CodeLocation? codeLocation = null)
                : base(message)
            {
                CodeLocation = codeLocation;
            }
        }

        public Converter(Stream errorOutputMessages)
        {
            _errorTextWriter = new StreamWriter(errorOutputMessages);
        }

        public static StateMachineSystem ConvertToStateMachine(Language.System system, Stream errorOutputMessages)
        {
            var converter = new Converter(errorOutputMessages);
            try
            {
                return converter.Convert(system);
            }
            catch (AbortConvertException e)
            {
                converter.WriteError(e.Message, e.CodeLocation);
                converter._errorTextWriter.Flush();
                return null;
            }
        }

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

        private Graph Convert(Language.Process process, string sendMessageServerName = null, List<Variable> variables = null, bool verifyConditionEdges = true)
        {
            var graph = new Graph() { Name = process.ServerName, AgentIndex = process.AgentIndex };

            sendMessageServerName ??= process.ServerName;
            variables ??= new List<Variable>();

            var initState = CreateInitState(variables);
            graph.InitNode = graph.GetOrCreateIdleNode(initState);

            var unhandledMessages = HandleCode(graph, graph.InitNode, process.Statements, InitCallerName, sendMessageServerName, variables, $"START_FROM_{InitCallerName}");
            foreach (var item in unhandledMessages)
            {
                graph.CreateEdge(item.Source, item.Source, item.Message, (sendMessageServerName, $"MISSING_CODE_{item.Source.PostCodeLocation}"));
            }

            return graph;
        }

        private void RemoveConditionFailingEdges(Graph graph)
        {
            var toRemove = new List<Edge>();
            foreach (var edge in graph.Edges)
            {
                if (edge.Condition == null) continue;
                if (IsConditionSatisfied(edge.Condition, edge.Source.States) == false)
                {
                    toRemove.Add(edge);
                }
            }

            foreach (var edgeToRemove in toRemove)
            {
                graph.Edges.Remove(edgeToRemove);
                edgeToRemove.Source.OutEdges.Remove(edgeToRemove);
                edgeToRemove.Target.InEdges.Remove(edgeToRemove);
            }
        }

        private Graph Convert(Language.Server server)
        {
            var graph = new Graph() { Name = server.Name };

            var initState = CreateInitState(server.Variables);

            graph.InitNode = graph.GetOrCreateIdleNode(initState);

            foreach (var action in server.Actions)
            {
                foreach (var branch in action.Branches)
                {
                    HandleServerActionBranch(graph, branch, action, server);
                }
            }

            RemoveConditionFailingEdges(graph);

            return graph;
        }

        private List<StatePair> CreateInitState(List<Variable> variables)
        {
            var initState = new List<StatePair>();
            foreach (var item in variables)
            {
                initState.Add(new StatePair(item));
            }

            return initState;
        }

        private void HandleServerActionBranch(Graph graph, ServerActionBranch branch, ServerAction action, Language.Server server)
        {
            foreach (var caller in action.Callers)
            {
                var preStates = GetCartesianStates(server.Variables, branch.Condition);

                foreach (var states in preStates)
                {
                    var beforeCallNode = graph.GetOrCreateIdleNode(states);
                    var unhandledMessages = HandleCode(graph, beforeCallNode, branch.Statements, caller, server.Name, server.Variables, $"CALL_{action.Name}_FROM_{caller}", verifyConditions: true, entranceDelay: branch.ExecutionDelay);
                    foreach (var item in unhandledMessages)
                    {
                        graph.CreateEdge(item.Source, item.Source, item.Message, (server.Name, $"MISSING_CODE_{item.Source.PostCodeLocation}"));
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

        private IEnumerable<PendingMessage> HandleCode(Graph graph, Node startNode, List<BaseStatement> statements, string caller, string serverName, List<Variable> variables, string receiveMessage, ICondition entranceCondition = null, bool verifyConditions = false, TimedDelay entranceDelay = null)
        {
            var currentStatementIndex = 0;
            var currentStatement = statements[currentStatementIndex];

            var firstNode = graph.GetOrCreateNode(startNode.States, caller, currentStatement.CodeLocation);
            var toHandle = new List<PendingMessage>();
            var newToHandle = new List<PendingMessage>();
            var firstEdge = graph.CreateEdge(
                startNode,
                firstNode,
                receiveMessage,
                (serverName, $"EXEC_{firstNode.CodeLocation}_FROM_{caller}"));
            firstEdge.Condition = entranceCondition;
            firstEdge.Delay = entranceDelay;
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
                            mutateEdge.Mutation = currentStatementMutation;
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
                                (serverName, $"PROCEED_{mutatedNodeAfter.PostCodeLocation}_FROM_{caller}"));
                            mutateEdge.Mutation = currentStatementMutation;
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
                            var nodesToHandle = HandleCode(graph, nodeAtCall, handler.HandlerStatements, caller, serverName, variables,
                                $"RETURN_{handler.HandledValue}", verifyConditions: verifyConditions);

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
                            var loopNode = graph.GetOrCreateNode(item.Source.States, caller, currentStatementLoop.CodeLocation, true);
                            var edge = graph.CreateEdge(item.Source, loopNode, item.Message, (serverName, $"PROCEED_{loopNode.CodeLocation}_FROM_{caller}"));

                            var nodesToHandle = HandleCode(graph, loopNode, currentStatementLoop.LoopStatements, caller, serverName, variables, edge.SendMessage, verifyConditions: verifyConditions);
                            newToHandle.AddRange(nodesToHandle);

                            handledPendingMessages.Add(item);
                        }
                        toHandle.Clear();
                        toHandle.AddRange(newToHandle);
                        newToHandle.Clear();

                        // remove already handled loop entrances
                        toHandle.RemoveAll(x => handledPendingMessages.Any(y => x.Message == y.Message && x.Source == y.Source));
                    }
                    return Enumerable.Empty<PendingMessage>(); // end of execution, cannot break from loop
                }
                else if (currentStatement is StatementWait currentStatementWait)
                {
                    foreach (var item in toHandle)
                    {
                        if (verifyConditions && !IsConditionSatisfied(currentStatementWait.Condition, item.Source.States))
                        {
                            continue;
                        }

                        if (nextStatement != null)
                        {
                            var nextNode = graph.GetOrCreateNode(item.Source.States, caller, nextStatement.CodeLocation);

                            var edge = graph.CreateEdge(item.Source, nextNode, item.Message,
                                (serverName, $"EXEC_{nextNode.CodeLocation}_FROM_{caller}"));
                            edge.Condition = currentStatementWait.Condition;
                            newToHandle.Add(PendingMessage.FromEdge(edge));
                        }
                        else
                        {
                            var nodeAtWait = graph.GetOrCreateNode(item.Source.States, caller, currentStatement.CodeLocation, true);
                            var edge = graph.CreateEdge(item.Source, nodeAtWait, item.Message,
                                    (serverName, $"PROCEED_{nodeAtWait.PostCodeLocation}_FROM_{caller}"));
                            edge.Condition = currentStatementWait.Condition;
                            newToHandle.Add(PendingMessage.FromEdge(edge));
                        }
                    }

                    if (nextStatement == null) return newToHandle;
                }
                else if (currentStatement is StatementIf currentStatementIf)
                {
                    foreach (var item in toHandle)
                    {
                        var notCondition = new ConditionNot() { Condition = currentStatementIf.Condition };
                        var nodesFromHandler = HandleCode(graph, item.Source, currentStatementIf.ConditionStatements, caller, serverName, variables, item.Message, currentStatementIf.Condition, verifyConditions: verifyConditions);

                        bool createEdgesForHandler = !verifyConditions || (verifyConditions && IsConditionSatisfied(currentStatementIf.Condition, item.Source.States));
                        bool createEdgesForNot = !verifyConditions || (verifyConditions && !IsConditionSatisfied(currentStatementIf.Condition, item.Source.States));

                        if (createEdgesForHandler)
                        {
                            if (nextStatement != null)
                            {
                                foreach (var itemFromHandler in nodesFromHandler)
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
                                newToHandle.AddRange(nodesFromHandler);
                            }
                        }

                        if (createEdgesForNot)
                        {
                            if (nextStatement != null)
                            {
                                var nextStatementNode = graph.GetOrCreateNode(item.Source.States, caller, nextStatement.CodeLocation);
                                var edge = graph.CreateEdge(item.Source, nextStatementNode,
                                    item.Message,
                                    (serverName, $"EXEC_{nextStatementNode.CodeLocation}_FROM_{caller}"));
                                edge.Condition = notCondition;
                                newToHandle.Add(PendingMessage.FromEdge(edge));
                            }
                            else
                            {
                                var nodeAtIf = graph.GetOrCreateNode(item.Source.States, caller, currentStatement.CodeLocation, true);
                                nodeAtIf.PostCodeLocation = currentStatement.PostCodeLocation;
                                var edge = graph.CreateEdge(item.Source, nodeAtIf,
                                    item.Message,
                                    (serverName, $"PROCEED_{nodeAtIf.PostCodeLocation}_FROM_{caller}"));
                                edge.Condition = notCondition;
                                newToHandle.Add(PendingMessage.FromEdge(edge));
                            }
                        }

                        if (nextStatement == null) return newToHandle;
                    }
                }
                else
                {
                    throw new NotImplementedException("Unsupported statement type");
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
            if (varStateIndex == -1) throw new AbortConvertException($"Cannot find variable {mutation.VariableName} in states", mutation.CodeLocation);
            var varState = copiedStates[varStateIndex];

            if (varState.VariableReference.Type == VariableType.Enum)
            {
                if (mutation.Operator != StateMutationOperator.Assignment)
                {
                    throw new AbortConvertException("Only assignment operator is available for enums", mutation.CodeLocation);
                }
                if (!varState.VariableReference.AvailableValues.Contains(mutation.Value))
                {
                    throw new AbortConvertException($"Enum variable '{varState.VariableReference.Name}' does not support value '{mutation.Value}'", mutation.CodeLocation);
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
                        throw new AbortConvertException($"Integer variable '{varState.VariableReference.Name}' out of range! Cannot '{varState.VariableReference.Name} = {mutation.Value}'", mutation.CodeLocation);
                    }
                    varState.Value = mutation.Value;
                }
                else if (mutation.Operator == StateMutationOperator.Increment)
                {
                    var newValue = intValue + mutationIntValue;
                    if (!varState.VariableReference.AvailableValues.Contains(newValue.ToString()))
                    {
                        throw new AbortConvertException($"Integer variable '{varState.VariableReference.Name}' out of range! Cannot '{varState.VariableReference.Name} += {mutationIntValue}' when '{varState.VariableReference.Name} == {intValue}'", mutation.CodeLocation);
                    }
                    varState.Value = newValue.ToString();
                }
                else if (mutation.Operator == StateMutationOperator.Decrement)
                {
                    var newValue = intValue - mutationIntValue;
                    if (!varState.VariableReference.AvailableValues.Contains(newValue.ToString()))
                    {
                        throw new AbortConvertException($"Integer variable '{varState.VariableReference.Name}' out of range! Cannot '{varState.VariableReference.Name} -= {mutationIntValue}' when '{varState.VariableReference.Name} == {intValue}'", mutation.CodeLocation);
                    }
                    varState.Value = newValue.ToString();
                }
                else if (mutation.Operator == StateMutationOperator.Modulo)
                {
                    var newValue = intValue % mutationIntValue;
                    if (!varState.VariableReference.AvailableValues.Contains(newValue.ToString()))
                    {
                        throw new AbortConvertException($"Integer variable '{varState.VariableReference.Name}' out of range! Cannot '{varState.VariableReference.Name} %= {mutationIntValue}' when '{varState.VariableReference.Name} == {intValue}'", mutation.CodeLocation);
                    }
                    varState.Value = newValue.ToString();
                }
                else
                {
                    throw new NotImplementedException("Mutation operator not supported");
                }
            }
            else
            {
                throw new NotImplementedException("Variable type not supported");
            }

            copiedStates[varStateIndex] = varState;

            return copiedStates;
        }

        public bool IsConditionSatisfied(ICondition condition, List<StatePair> states)
        {
            if (condition == null) return IsConditionSatisfied(condition, states);
            else if (condition is ConditionNot) return !IsConditionSatisfied((condition as ConditionNot).Condition, states);
            else if (condition is ConditionNode) return IsConditionSatisfied(condition as ConditionNode, states);
            else if (condition is ConditionLeaf) return IsConditionSatisfiedLeaf(condition as ConditionLeaf, states);

            throw new NotImplementedException("Unsupported condition type");
        }

        public bool IsConditionSatisfied(ConditionNode conditionNode, List<StatePair> states)
        {
            var isLeftSatisfied = IsConditionSatisfied(conditionNode.Left, states);
            var isRightSatisfied = IsConditionSatisfied(conditionNode.Right, states);

            if (conditionNode.Operator == ConditionLogicalOperator.And) return isLeftSatisfied && isRightSatisfied;
            else if (conditionNode.Operator == ConditionLogicalOperator.Or) return isLeftSatisfied || isRightSatisfied;

            throw new NotImplementedException("Unsupported condition logic operator");
        }

        public bool IsConditionSatisfiedLeaf(ConditionLeaf condition, List<StatePair> states)
        {
            if (!states.Any(x => x.Name == condition.VariableName))
            {
                throw new AbortConvertException($"Unknown variable '{condition.VariableName}'", condition.CodeLocation);
            }
            var state = states.Single(x => x.Name == condition.VariableName);
            if (state.VariableReference.Type != condition.VariableType)
            {
                throw new AbortConvertException($"Incorrect variable type '{condition.VariableName}'", condition.CodeLocation);
            }

            return CheckCondition(condition, state.Value);
        }

        public List<List<StatePair>> GetCartesianStates(List<Variable> variables, ICondition condition = null)
        {
            if (condition == null) return GetCartesianStatesLeaf(variables, null);
            else if (condition is ConditionNode) return GetCartesianStates(variables, condition as ConditionNode);
            else if (condition is ConditionLeaf) return GetCartesianStatesLeaf(variables, condition as ConditionLeaf);

            throw new NotImplementedException("Unsupported condition type");
        }

        public List<List<StatePair>> GetCartesianStates(List<Variable> variables, ConditionNode conditionNode)
        {
            var leftStates = GetCartesianStates(variables, conditionNode.Left);
            var rightStates = GetCartesianStates(variables, conditionNode.Right);

            if (conditionNode.Operator == ConditionLogicalOperator.And) return leftStates.Intersect(rightStates, _listStatePairEqualityComparer).ToList();
            else if (conditionNode.Operator == ConditionLogicalOperator.Or) return leftStates.Union(rightStates, _listStatePairEqualityComparer).Distinct(_listStatePairEqualityComparer).ToList();

            throw new NotImplementedException("Unsupported condition logic operator");
        }

        public List<List<StatePair>> GetCartesianStatesLeaf(List<Variable> variables, ConditionLeaf condition)
        {
            var states = new List<List<StatePair>>() { new List<StatePair>() };

            foreach (var variable in variables)
            {
                var newStates = new List<List<StatePair>>();
                foreach (var state in states)
                {
                    foreach (var value in variable.AvailableValues)
                    {
                        bool conditionSatisfied = true;
                        if (condition != null && condition.VariableName == variable.Name)
                        {
                            if (condition.VariableType != variable.Type) throw new AbortConvertException($"Incorrect condition variable type for variable '{variable.Name}'", condition.CodeLocation);
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
                    default: throw new NotImplementedException("Operator not supported");
                }
            }
            else if (condition.VariableType == VariableType.Enum)
            {
                switch (condition.Operator)
                {
                    case ConditionOperator.Equal: return variableValue == condition.Value;
                    case ConditionOperator.NotEqual: return variableValue != condition.Value;
                    default: throw new NotImplementedException("Operator not supported");
                }
            }
            else
            {
                throw new NotImplementedException("Unknown VariableType");
            }
        }

        private ComposedGraph Convert(Language.Group group)
        {
            var result = new ComposedGraph() { Name = group.ServerName, RequiredAgents = group.Processes.Select(x => x.AgentIndex).ToArray() };

            var processesGraphs = group.Processes.Select(x => Convert(x, group.ServerName, group.Variables, false)).ToList();
            var initState = CreateInitState(group.Variables);

            result.InitNode = new ComposedNode() { States = new List<StatePair>(initState) };
            foreach (var graph in processesGraphs)
            {
                var agentState = new ComposedNode.AgentState() { BaseNodeReference = graph.InitNode };
                result.InitNode.Agents.Add(graph.AgentIndex.Value, agentState);
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
                        if (baseEdge.Condition != null && IsConditionSatisfied(baseEdge.Condition, node.States) == false)
                        {
                            continue;
                        }

                        var nextBaseNodes = GetBaseNodesWithAgents(node);
                        nextBaseNodes[agentIndex] = baseEdge.Target;

                        var nextStates = baseEdge.Mutation == null ? node.States : Mutate(node.States, baseEdge.Mutation);

                        var nextNode = result.GetOrCreateNode(nextBaseNodes, nextStates, out var isNew);
                        result.GetOrCreateEdge(agentIndex, node, nextNode, baseEdge.ReceiveMessage, (baseEdge.SendMessageServer, baseEdge.SendMessage), baseEdge.Delay, out _);
                        if (isNew)
                        {
                            toProcess.Push(nextNode);
                        }
                    }
                }
            }

            return result;
        }

        private Dictionary<int, Node> GetBaseNodesWithAgents(ComposedNode composedNode)
        {
            return composedNode.Agents.ToDictionary(x => x.Key, x => x.Value.BaseNodeReference);
        }

        public IEnumerable<List<Node>> GetNodeCombinations(List<Graph> graphs)
        {
            var currentGraph = graphs.ElementAt(0);

            if (graphs.Count == 1)
            {
                foreach (var node in currentGraph.Nodes)
                {
                    yield return new List<Node>() { node };
                }
                yield break;
            }

            var restGraphs = graphs.Skip(1).ToList();
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

        private void WriteError(string message, CodeLocation? codeLocation = null)
        {
            string codeLocMsg = codeLocation.HasValue ? $"L: {codeLocation.Value.StartLine} C: {codeLocation.Value.StartColumn + 1} - " : "";
            _errorTextWriter.WriteLine($"{codeLocMsg}Convert error - {message}");
        }
    }
}
