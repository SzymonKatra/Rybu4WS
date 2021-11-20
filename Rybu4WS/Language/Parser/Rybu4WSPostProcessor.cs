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

        public Rybu4WSPostProcessor(TextWriter errorTextWriter)
        {
            _errorTextWriter = errorTextWriter;
        }

        public void Process(Language.System system)
        {
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

                if (dependantServer.Type != serverDeclaration.Dependencies[i].Type)
                {
                    WriteError($"Server ${dependantServer.Name} is of type {dependantServer.Type} but type {serverDeclaration.Dependencies[i].Type} is required", serverDefinition.CodeLocation);
                    continue;
                }

                dependencyMapping.Add(serverDeclaration.Dependencies[i].Name, dependencyName);
            }

            var server = new Server()
            {
                Name = serverDefinition.Name,
                Type = serverDeclaration.TypeName
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
                server.Variables.Add(new ServerVariable()
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
                    actionBranch.Statements = branchDeclaration.Statements.Select(x => CloneAndMap(x, serverDeclaration, dependencyMapping)).ToList();

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
                if (!dependencyMapping.TryGetValue(declaredCall.ServerName, out var realServerName))
                {
                    WriteError($"Unknown dependency '{declaredCall.ServerName}' in server of type '{serverDeclaration.TypeName}'", declaredStatement.CodeLocation);
                    return null;
                }

                return new StatementCall()
                {
                    ServerName = realServerName ?? "",
                    ActionName = declaredCall.ActionName,
                    CodeLocation = declaredCall.CodeLocation
                };
            }
            else if (declaredStatement is StatementMatch declaredMatch)
            {
                if (!dependencyMapping.TryGetValue(declaredMatch.ServerName, out var realServerName))
                {
                    WriteError($"Unknown dependency '{declaredMatch.ServerName}' in server of type '{serverDeclaration.TypeName}'", declaredStatement.CodeLocation);
                    return null;
                }

                return new StatementMatch()
                {
                    ServerName = realServerName ?? "",
                    ActionName = declaredMatch.ActionName,
                    Handlers = declaredMatch.Handlers.Select(h => new StatementMatchOption()
                    {
                        HandledValue = h.HandledValue,
                        HandlerStatements = h.HandlerStatements.Select(hs => CloneAndMap(hs, serverDeclaration, dependencyMapping)).ToList()
                    }).ToList(),
                    CodeLocation = declaredMatch.CodeLocation
                };
            }
            else if (declaredStatement is StatementStateMutation declaredStateMutation)
            {
                return new StatementStateMutation()
                {
                    VariableName = declaredStateMutation.VariableName,
                    Operator = declaredStateMutation.Operator,
                    Value = declaredStateMutation.Value,
                    CodeLocation = declaredStatement.CodeLocation
                };
            }
            else if (declaredStatement is StatementReturn declaredReturn)
            {
                return new StatementReturn()
                {
                    Value = declaredReturn.Value,
                    CodeLocation = declaredReturn.CodeLocation
                };
            }
            else if (declaredStatement is StatementTerminate declaredTerminate)
            {
                return new StatementTerminate()
                {
                    CodeLocation = declaredTerminate.CodeLocation
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
                    if (branch.Condition != null)
                    {
                        ValidateCondition(server, branch.Condition);
                    }

                    action.PossibleReturnValues.AddRange(GetStatements<Language.StatementReturn>(branch.Statements).Select(x => x.Value));

                    FillReferences(system, server.Name, branch.Statements);
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
                WriteError($"Variable '{conditionLeaf.VariableName}' does not exist in server '{server.Name}'");
            }
            
            if (serverVariable.Type != conditionLeaf.VariableType)
            {
                WriteError($"Value in condition has type {Enum.GetName(typeof(VariableType), conditionLeaf.VariableType)} but variable is of type {Enum.GetName(typeof(VariableType), serverVariable.Type)}");
            }
        }

        private void ProcessProcess(Language.System system, Process process)
        {
            FillReferences(system, process.ServerName, process.Statements);
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

        private List<T> GetStatements<T>(List<BaseStatement> statements) where T : BaseStatement
        {
            var result = new List<T>();

            foreach (var statement in statements)
            {
                if (statement is Language.StatementMatch)
                {
                    var matchStmt = statement as Language.StatementMatch;
                    foreach (var handler in matchStmt.Handlers)
                    {
                        result.AddRange(GetStatements<T>(handler.HandlerStatements));
                    }
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
            string codeLocMsg = codeLocation.HasValue ? $" AT (Start {codeLocation.Value.StartLine}:{codeLocation.Value.StartColumn}, Stop: {codeLocation.Value.EndLine}:{codeLocation.Value.EndColumn})" : "";
            _errorTextWriter.WriteLine($"POST-PROCESSOR ERROR{codeLocMsg} - {message}");
        }
    }
}
