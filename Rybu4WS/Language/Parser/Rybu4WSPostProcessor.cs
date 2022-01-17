using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.Language.Parser
{
    public class Rybu4WSPostProcessor
    {
        private TextWriter _errorTextWriter;
        private int _currentAgentIndex = 1;

        public Rybu4WSPostProcessor(TextWriter errorTextWriter)
        {
            _errorTextWriter = errorTextWriter;
        }

        public void Process(Language.System system)
        {
            foreach (var serverDeclaration in system.ServerDeclarations)
            {
                ValidateInterfaces(system, serverDeclaration);
                ValidateDependencies(system, serverDeclaration);
                ValidateCalls(system, serverDeclaration);
            }

            foreach (var serverDefinition in system.ServerDefinitions)
            {
                CreateServer(system, serverDefinition);
            }

            foreach (var server in system.Servers)
            {
                ProcessServer(system, server);
            }

            foreach (var process in system.Processes)
            {
                ProcessProcess(system, process);
            }

            foreach (var group in system.Groups)
            {
                ProcessGroup(system, group);
            }

            ValidateChannels(system);

            _errorTextWriter.Flush();
        }

        private void CreateServer(Language.System system, ServerDefinition serverDefinition)
        {
            var serverDeclaration = system.ServerDeclarations.SingleOrDefault(x => x.TypeName == serverDefinition.Type);
            if (serverDefinition == null)
            {
                WriteError($"Cannot instantiate server '{serverDefinition.Name}' because type '{serverDefinition.Type}' is not declared", serverDefinition.CodeLocation);
                return;
            }

            if (serverDefinition.DependencyNameList.Count != serverDeclaration.Dependencies.Count)
            {
                WriteError($"Server type '{serverDeclaration.TypeName}' requires {serverDeclaration.Dependencies.Count} dependencies but provided {serverDefinition.DependencyNameList.Count}", serverDefinition.CodeLocation);
                return;
            }

            // key = server declaration dependency name
            // value = real instance name
            var dependencyMapping = new Dictionary<string, string>();
            for (int i = 0; i < serverDefinition.DependencyNameList.Count; i++)
            {
                var dependencyName = serverDefinition.DependencyNameList[i];

                var dependantServer = system.Servers.SingleOrDefault(x => x.Name == dependencyName);
                if (dependantServer == null)
                {
                    WriteError($"Server '{dependencyName}' is not defined", serverDefinition.CodeLocation);
                    continue;
                }

                var requiredType = serverDeclaration.Dependencies[i].Type;

                if (dependantServer.Type != requiredType)
                {
                    if (system.InterfaceDeclarations.Any(x => x.TypeName == requiredType))
                    {
                        if (!dependantServer.ImplementedInterfaces.Contains(requiredType))
                        {
                            WriteError($"Server '{dependantServer.Name}' of type '{dependantServer.Type}' does not implement interface '{requiredType}'", serverDefinition.CodeLocation);
                        }
                    }
                    else
                    {
                        WriteError($"Server '{dependantServer.Name}' is of type '{dependantServer.Type}' but type '{requiredType}' is required", serverDefinition.CodeLocation);
                        continue;
                    }
                }

                dependencyMapping.Add(serverDeclaration.Dependencies[i].Name, dependencyName);
            }

            var server = new Server()
            {
                Name = serverDefinition.Name,
                Type = serverDeclaration.TypeName,
                ImplementedInterfaces = serverDeclaration.ImplementedInterfaces.Select(x => x.InterfaceTypeName).ToList()
            };
            foreach (var variable in serverDeclaration.Variables)
            {
                if (!serverDefinition.VariablesInitialValues.TryGetValue(variable.Name, out var initValue))
                {
                    WriteError($"Missing initial variable value for '{variable.Name}'", serverDefinition.CodeLocation);
                    continue;
                }
                if (!variable.AvailableValues.Contains(initValue))
                {
                    WriteError($"Variable '{variable.Name}' cannot get value '{initValue}'", serverDefinition.CodeLocation);
                }

                server.Variables.Add(new Variable()
                {
                    Name = variable.Name,
                    Type = variable.Type,
                    AvailableValues = new List<string>(variable.AvailableValues),
                    InitialValue = initValue
                });
            }
            foreach (var actionDeclaration in serverDeclaration.Actions)
            {
                var action = new ServerAction() { Name = actionDeclaration.Name };
                foreach (var branchDeclaration in actionDeclaration.Branches)
                {
                    var actionBranch = new ServerActionBranch();
                    actionBranch.Condition = branchDeclaration.Condition?.Clone();
                    if (actionBranch.Condition != null)
                    {
                        ValidateCondition(server, actionBranch.Condition);
                    }

                    actionBranch.ExecutionDelay = branchDeclaration.ExecutionDelay?.Clone();
                    actionBranch.Statements = branchDeclaration.Statements.Select(x => CloneAndMap(x, serverDeclaration, dependencyMapping)).ToList();
                    action.PossibleReturnValues.AddRange(GetStatements<Language.StatementReturn>(actionBranch.Statements).Select(x => x.Value));

                    action.Branches.Add(actionBranch);
                }
                server.Actions.Add(action);
            }

            system.Servers.Add(server);
        }

        private BaseStatement CloneAndMap(BaseStatement declaredStatement, ServerDeclaration serverDeclaration, Dictionary<string, string> dependencyMapping)
        {
            if (declaredStatement is StatementCall declaredCall)
            {
                if (!serverDeclaration.Dependencies.Any(x => x.Name == declaredCall.ServerName))
                {
                    WriteError($"Unknown dependency '{declaredCall.ServerName}' in server of type '{serverDeclaration.TypeName}'", declaredStatement.CodeLocation);
                    return null;
                }

                if (!dependencyMapping.TryGetValue(declaredCall.ServerName, out var realServerName))
                {
                    WriteError($"Server instance for '{declaredCall.ServerName}' dependency in server of type '{serverDeclaration.TypeName}' is not defined", declaredStatement.CodeLocation);
                    return null;
                }

                return new StatementCall()
                {
                    ServerName = realServerName ?? "",
                    ActionName = declaredCall.ActionName,
                    CodeLocation = declaredCall.CodeLocation,
                    PostCodeLocation = declaredCall.PostCodeLocation
                };
            }
            else if (declaredStatement is StatementMatch declaredMatch)
            {
                if (!serverDeclaration.Dependencies.Any(x => x.Name == declaredMatch.ServerName))
                {
                    WriteError($"Unknown dependency '{declaredMatch.ServerName}' in server of type '{serverDeclaration.TypeName}'", declaredStatement.CodeLocation);
                    return null;
                }

                if (!dependencyMapping.TryGetValue(declaredMatch.ServerName, out var realServerName))
                {
                    WriteError($"Server instance for '{declaredMatch.ServerName}' dependency in server of type '{serverDeclaration.TypeName}' is not defined", declaredStatement.CodeLocation);
                    return null;
                }

                return new StatementMatch()
                {
                    ServerName = realServerName ?? "",
                    ActionName = declaredMatch.ActionName,
                    Handlers = declaredMatch.Handlers.Select(h => new StatementMatchOption()
                    {
                        HandledValue = h.HandledValue,
                        HandlerStatements = h.HandlerStatements.Select(hs => CloneAndMap(hs, serverDeclaration, dependencyMapping)).ToList(),
                        CodeLocation = h.CodeLocation
                    }).ToList(),
                    CodeLocation = declaredMatch.CodeLocation,
                    PostCodeLocation = declaredMatch.PostCodeLocation
                };
            }
            else if (declaredStatement is StatementStateMutation declaredStateMutation)
            {
                return new StatementStateMutation()
                {
                    VariableName = declaredStateMutation.VariableName,
                    Operator = declaredStateMutation.Operator,
                    Value = declaredStateMutation.Value,
                    CodeLocation = declaredStatement.CodeLocation,
                    PostCodeLocation = declaredStatement.PostCodeLocation
                };
            }
            else if (declaredStatement is StatementReturn declaredReturn)
            {
                return new StatementReturn()
                {
                    Value = declaredReturn.Value,
                    CodeLocation = declaredReturn.CodeLocation,
                    PostCodeLocation = declaredReturn.PostCodeLocation
                };
            }
            else if (declaredStatement is StatementTerminate declaredTerminate)
            {
                return new StatementTerminate()
                {
                    CodeLocation = declaredTerminate.CodeLocation,
                    PostCodeLocation = declaredTerminate.PostCodeLocation
                };
            }
            else if (declaredStatement is StatementLoop declaredLoop)
            {
                return new StatementLoop()
                {
                    LoopStatements = declaredLoop.LoopStatements.Select(s => CloneAndMap(s, serverDeclaration, dependencyMapping)).ToList(),
                    CodeLocation = declaredLoop.CodeLocation,
                    PostCodeLocation = declaredLoop.PostCodeLocation
                };
            }
            else if (declaredStatement is StatementWait declaredWait)
            {
                return new StatementWait()
                {
                    Condition = declaredWait.Condition.Clone(),
                    CodeLocation = declaredWait.CodeLocation,
                    PostCodeLocation = declaredWait.PostCodeLocation
                };
            }
            else if (declaredStatement is StatementIf declaredIf)
            {
                return new StatementIf()
                {
                    Condition = declaredIf.Condition.Clone(),
                    ConditionStatements = declaredIf.ConditionStatements.Select(s => CloneAndMap(s, serverDeclaration, dependencyMapping)).ToList(),
                    CodeLocation = declaredIf.CodeLocation,
                    PostCodeLocation = declaredIf.PostCodeLocation
                };
            }
            else
            {
                throw new NotImplementedException("Unsupported statement type");
            }
        }

        private void ProcessServer(Language.System system, Server server)
        {
            foreach (var action in server.Actions)
            {
                foreach (var branch in action.Branches)
                {
                    FillReferences(system, server.Name, branch.Statements);
                    ValidateMatchHandlers(system, branch.Statements);
                }
            }

            foreach (var action in server.Actions)
            {
                FillTerminations(action);
            }
        }

        private void ValidateMatchHandlers(Language.System system, IEnumerable<Language.BaseStatement> statements)
        {
            foreach (var matchStatement in GetStatements<Language.StatementMatch>(statements))
            {
                var server = system.Servers.Single(x => x.Name == matchStatement.ServerName); // should not throw because
                var action = server.Actions.Single(x => x.Name == matchStatement.ActionName); // it should be validated earlier

                foreach (var matchOption in matchStatement.Handlers)
                {
                    if (!action.PossibleReturnValues.Contains(matchOption.HandledValue))
                    {
                        WriteError($"Handling '{matchOption.HandledValue}', but {server.Name}.{action.Name} never returns such value", matchOption.CodeLocation);
                    }
                }
            }
        }

        private void FillTerminations(ServerAction action)
        {
            if (action.CanTerminate)
            {
                return;
            }

            var statements = GetStatements<BaseStatement>(action.Branches.SelectMany(x => x.Statements));
            foreach (var statement in statements)
            {
                if (statement is StatementTerminate)
                {
                    action.CanTerminate = true;
                    return;
                }
                else if (statement is StatementCall statementCall)
                {
                    FillTerminations(statementCall.ServerActionReference);
                    if (statementCall.ServerActionReference.CanTerminate)
                    {
                        action.CanTerminate = true;
                        return;
                    }
                }
                else if (statement is StatementMatch statementMatch)
                {
                    FillTerminations(statementMatch.ServerActionReference);
                    if (statementMatch.ServerActionReference.CanTerminate)
                    {
                        action.CanTerminate = true;
                        return;
                    }
                }
            }
        }

        private void ValidateInterfaces(Language.System system, ServerDeclaration serverDeclaration)
        {
            foreach (var iface in serverDeclaration.ImplementedInterfaces)
            {
                iface.InterfaceDeclarationReference = system.InterfaceDeclarations.SingleOrDefault(x => x.TypeName == iface.InterfaceTypeName);
                if (iface.InterfaceDeclarationReference == null)
                {
                    WriteError($"Interface '{iface.InterfaceTypeName}' not found", serverDeclaration.CodeLocation);
                    continue;
                }

                foreach (var ifaceAction in iface.InterfaceDeclarationReference.RequiredActions)
                {
                    var serverActions = serverDeclaration.Actions.Where(x => x.Name == ifaceAction.Name);
                    if (!serverActions.Any())
                    {
                        WriteError($"Server '{serverDeclaration.TypeName}' does not implement action {ifaceAction.Name} of interface '{iface.InterfaceDeclarationReference.TypeName}'", serverDeclaration.CodeLocation);
                        continue;
                    }

                    var returnValues = GetStatements<StatementReturn>(serverActions.SelectMany(x => x.Branches).SelectMany(x => x.Statements))
                        .Select(x => x.Value).ToList();

                    foreach (var retVal in returnValues)
                    {
                        if (!ifaceAction.PossibleReturnValues.Contains(retVal))
                        {
                            WriteError($"Action '{ifaceAction.Name}' in server '{serverDeclaration.TypeName}' returns value '{retVal}' but '{iface.InterfaceDeclarationReference.TypeName}.{ifaceAction.Name}' doesn't define it as an return value", serverDeclaration.CodeLocation);
                        }
                    }
                }
            }
        }

        private void ValidateDependencies(Language.System system, ServerDeclaration serverDeclaration)
        {
            foreach (var dependency in serverDeclaration.Dependencies)
            {
                var iface = system.InterfaceDeclarations.SingleOrDefault(x => x.TypeName == dependency.Type);
                var serverDecl = system.ServerDeclarations.SingleOrDefault(x => x.TypeName == dependency.Type);

                if (iface == null && serverDecl == null)
                {
                    WriteError($"Unknown type '{dependency.Type}'", serverDeclaration.CodeLocation);
                }
            }
        }

        private void ValidateCalls(Language.System system, ServerDeclaration serverDeclaration)
        {
            var topLevelStatements = serverDeclaration.Actions.SelectMany(x => x.Branches).SelectMany(x => x.Statements).ToList();

            var calls = GetStatements<StatementCall>(topLevelStatements).Select(x => (x.ServerName, x.ActionName, StatementCall: x, StatementMatch: (Language.StatementMatch)null)).ToList();
            calls.AddRange(GetStatements<StatementMatch>(topLevelStatements).Select(x => (x.ServerName, x.ActionName, StatementCall: (StatementCall)null, StateentMatch: x)));

            foreach (var call in calls)
            {
                var dependency = serverDeclaration.Dependencies.SingleOrDefault(x => x.Name == call.ServerName);
                var codeLoc = call.StatementCall?.CodeLocation ?? call.StatementMatch?.CodeLocation;
                if (dependency == null)
                {
                    WriteError($"Unknown server name '{call.ServerName}'", codeLoc);
                    continue;
                }

                var iface = system.InterfaceDeclarations.SingleOrDefault(x => x.TypeName == dependency.Type);
                var serverDecl = system.ServerDeclarations.SingleOrDefault(x => x.TypeName == dependency.Type);

                if (iface != null)
                {
                    if (!iface.RequiredActions.Any(x => x.Name == call.ActionName))
                    {
                        WriteError($"Interface of type '{iface.TypeName}' doesn't have action named '{call.ActionName}'", codeLoc);
                    }
                }
                else if (serverDecl != null)
                {
                    if (!serverDecl.Actions.Any(x => x.Name == call.ActionName))
                    {
                        WriteError($"Server of type '{serverDecl.TypeName}' doesn't have action named '{call.ActionName}'", codeLoc);
                    }
                }
                else
                {
                    throw new Exception($"Unknown type '{dependency.Type}'");
                }
            }
        }

        private void ValidateCondition(Server server, ICondition condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException("Passed null to ValidateCondition");
            }

            if (condition is ConditionLeaf leaf)
            {
                ValidateConditionLeaf(server, leaf);
                return;
            }
            else if (condition is ConditionNode node)
            {
                ValidateCondition(server, node.Left);
                ValidateCondition(server, node.Right);
            }
            else
            {
                throw new NotImplementedException("Unknown condition type");
            }
        }

        private void ValidateConditionLeaf(Server server, ConditionLeaf conditionLeaf)
        {
            var serverVariable = server.Variables.SingleOrDefault(x => x.Name == conditionLeaf.VariableName);
            if (serverVariable == null)
            {
                WriteError($"Variable '{conditionLeaf.VariableName}' does not exist in server '{server.Name}'", conditionLeaf.CodeLocation);
                return;
            }
            
            if (serverVariable.Type != conditionLeaf.VariableType)
            {
                WriteError($"Value in condition has type {Enum.GetName(typeof(VariableType), conditionLeaf.VariableType)} but variable is of type {Enum.GetName(typeof(VariableType), serverVariable.Type)}", conditionLeaf.CodeLocation);
                return;
            }
        }

        private void ValidateChannels(Language.System system)
        {
            foreach (var channel in system.TimedChannels)
            {
                if (channel.SourceServer != null)
                {
                    if (system.Servers.Any(x => x.Name == channel.SourceServer) == false)
                    {
                        WriteError($"Channel validation failed, server {channel.SourceServer} does not exist in the system!", channel.CodeLocation);
                    }
                }

                if (channel.TargetServer != null)
                {
                    if (system.Servers.Any(x => x.Name == channel.TargetServer) == false)
                    {
                        WriteError($"Channel validation failed, server {channel.TargetServer} does not exist in the system!", channel.CodeLocation);
                    }
                }
            }

            Func<ChannelDefinition, bool> globalDelayPredicate = x => x.SourceServer == null && x.TargetServer == null;
            if (system.TimedChannels.Count(globalDelayPredicate) > 1)
            {
                var lastGlobalTimedChannel = system.TimedChannels.Last(globalDelayPredicate);
                WriteError($"More than one global delay was defined in channels. Please define single global delay", lastGlobalTimedChannel.CodeLocation);
            }
        }

        private void ProcessProcess(Language.System system, Process process)
        {
            FillReferences(system, process.ServerName, process.Statements);
            process.AgentIndex = _currentAgentIndex;
            _currentAgentIndex++;

            if (process.Statements.Count == 0)
            {
                WriteError($"Process '{process.Name}' must have at least one statement", process.CodeLocation);
            }
            ValidateMatchHandlers(system, process.Statements);
        }

        private void ProcessGroup(Language.System system, Group group)
        {
            foreach (var process in group.Processes)
            {
                FillReferences(system, group.ServerName, process.Statements);
                process.AgentIndex = _currentAgentIndex;
                _currentAgentIndex++;

                if (process.Statements.Count == 0)
                {
                    WriteError($"Process '{process.Name}' must have at least one statement", process.CodeLocation);
                }
                ValidateMatchHandlers(system, process.Statements);
            }
        }

        private void FillReferences(Language.System system, string callerName, List<BaseStatement> statements)
        {
            var calls = GetStatements<StatementCall>(statements).Select(x => (x.ServerName, x.ActionName, StatementCall: x, StatementMatch: (Language.StatementMatch)null)).ToList();
            calls.AddRange(GetStatements<StatementMatch>(statements).Select(x => (x.ServerName, x.ActionName, StatementCall: (StatementCall)null, StateentMatch: x)));

            foreach (var call in calls)
            {
                var calledServer = system.Servers.SingleOrDefault(x => x.Name == call.ServerName);
                if (calledServer == null)
                {
                    WriteError($"Unknown server name {call.ServerName}", call.StatementCall?.CodeLocation ?? call.StatementMatch?.CodeLocation);
                    continue;
                }
                var calledAction = calledServer.Actions.SingleOrDefault(x => x.Name == call.ActionName);
                if (calledAction == null)
                {
                    WriteError($"Unknown action name {call.ActionName}", call.StatementCall?.CodeLocation ?? call.StatementMatch?.CodeLocation);
                    continue;
                }
                if (!calledAction.Callers.Contains(callerName))
                {
                    calledAction.Callers.Add(callerName);
                }

                if (call.StatementCall != null)
                {
                    call.StatementCall.ServerActionReference = calledAction;
                }
                if (call.StatementMatch != null)
                {
                    call.StatementMatch.ServerActionReference = calledAction;
                }
            }
        }

        private List<T> GetStatements<T>(IEnumerable<BaseStatement> statements) where T : BaseStatement
        {
            var result = new List<T>();

            foreach (var statement in statements)
            {
                if (statement is Language.StatementMatch matchStatement)
                {
                    foreach (var handler in matchStatement.Handlers)
                    {
                        result.AddRange(GetStatements<T>(handler.HandlerStatements));
                    }
                }
                if (statement is Language.StatementLoop loopStatement)
                {
                    result.AddRange(GetStatements<T>(loopStatement.LoopStatements));
                }
                if (statement is Language.StatementIf ifStatement)
                {
                    result.AddRange(GetStatements<T>(ifStatement.ConditionStatements));
                }
                if (statement is T)
                {
                    result.Add(statement as T);
                }
            }

            return result;
        }

        private void WriteError(string message, CodeLocation? codeLocation = null)
        {
            string codeLocMsg = codeLocation.HasValue ? $"L: {codeLocation.Value.StartLine} C: {codeLocation.Value.StartColumn + 1} - " : "";
            _errorTextWriter.WriteLine($"{codeLocMsg}post processing error - {message}");
        }
    }
}
