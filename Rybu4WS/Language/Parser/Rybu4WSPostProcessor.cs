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
            foreach (var server in system.Servers)
            {
                ProcessServer(system, server);
            }

            foreach (var process in system.Processes)
            {
                ProcessProcess(system, process);
            }
        }

        private void ProcessServer(Language.System system, Server server)
        {
            foreach (var action in server.Actions)
            {
                foreach (var branch in action.Branches)
                {
                    action.PossibleReturnValues.AddRange(GetStatements<Language.StatementReturn>(branch.Statements).Select(x => x.Value));

                    FillReferences(system, server.Name, branch.Statements);
                }
            }
        }

        private void ProcessProcess(Language.System system, Process process)
        {
            FillReferences(system, process.Name, process.Statements);
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
