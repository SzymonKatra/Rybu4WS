using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS
{
    public class PostProcessor
    {
        public void Process(Logic.System system)
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

        private void ProcessServer(Logic.System system, Logic.Server server)
        {
            foreach (var action in server.Actions)
            {
                foreach (var branch in action.Branches)
                {
                    action.PossibleReturnValues.AddRange(GetStatements<Logic.StatementReturn>(branch.Statements).Select(x => x.Value));

                    FillReferences(system, server.Name, branch.Statements);
                }
            }
        }

        private void ProcessProcess(Logic.System system, Logic.Process process)
        {
            FillReferences(system, process.Name, process.Statements);
        }

        private void FillReferences(Logic.System system, string callerName, List<Logic.BaseStatement> statements)
        {
            var calls = GetStatements<Logic.StatementCall>(statements).Select(x => (x.ServerName, x.ActionName, StatementCall: x, StatementMatch: (Logic.StatementMatch)null)).ToList();
            calls.AddRange(GetStatements<Logic.StatementMatch>(statements).Select(x => (x.ServerName, x.ActionName, StatementCall: (Logic.StatementCall)null, StateentMatch: x)));

            foreach (var call in calls)
            {
                var calledServer = system.Servers.SingleOrDefault(x => x.Name == call.ServerName);
                if (calledServer == null) throw new Exception($"Unknown server name {call.ServerName}");
                var calledAction = calledServer.Actions.SingleOrDefault(x => x.Name == call.ActionName);
                if (calledAction == null) throw new Exception($"Unknown action name {call.ActionName}");
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

        public List<T> GetStatements<T>(List<Logic.BaseStatement> statements) where T : Logic.BaseStatement
        {
            var result = new List<T>();

            foreach (var statement in statements)
            {
                if (statement is Logic.StatementMatch)
                {
                    var matchStmt = statement as Logic.StatementMatch;
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
    }
}
