using Rybu4WS.Language;
using Rybu4WS.TrailDebugger.TrailSchema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Rybu4WS.TrailDebugger
{
    public class Debugger
    {
        private class AgentDebugState
        {
            public List<AgentTraceEntry> Trace { get; set; } = new List<AgentTraceEntry>();

            public TrailSchema.Message LastMessage { get; set; }
        }

        public static readonly XmlSerializer Serializer = new XmlSerializer(typeof(Trail), new XmlRootAttribute("trail"));

        private Trail _trail;
        private int _currentConfigurationIndex;

        private Dictionary<string, string> _serverStates;
        private Dictionary<string, AgentDebugState> _agentTraces;

        public Debugger(string input)
        {
            _trail = Parse(input);
            FillReferences();
            Reset();
        }

        public void Reset()
        {
            _serverStates = new Dictionary<string, string>();
            foreach (var server in _trail.ServerVars)
            {
                _serverStates.Add(server.ServerVarName, server.ServerVarState);
            }

            _agentTraces = new Dictionary<string, AgentDebugState>();
            foreach (var agent in _trail.AgentVars)
            {
                _agentTraces.Add(GetIndexedName(agent.AgentVarName), new AgentDebugState());
            }

            _currentConfigurationIndex = -1;
        }

        public bool CanStep()
        {
            return _currentConfigurationIndex + 1 < _trail.Configurations.Length;
        }

        public (string serverNameUpdate, string agentNameUpdate) Step()
        {
            if (!CanStep()) throw new Exception("Cannot step further!");

            _currentConfigurationIndex++;
            var trailConfig = _trail.Configurations[_currentConfigurationIndex];

            string serverChanged = null;
            string agentChanged = null;

            foreach (var state in trailConfig.States)
            {
                if (_serverStates[state.Server] != state.Value)
                {
                    if (serverChanged != null)
                    {
                        throw new Exception("State in more than one server has changed during step!");
                    }

                    _serverStates[state.Server] = state.Value;
                    serverChanged = state.Server;
                }
            }

            if (serverChanged != null && serverChanged.StartsWith("Process"))
            {
                serverChanged = null;
            }

            foreach (var message in trailConfig.Messages)
            {
                var agentName = GetIndexedName(message.Agent);

                var agentState = _agentTraces[agentName];
                if (CompareMessages(message, agentState.LastMessage)) continue;

                if (message.Service == "START_FROM_INIT")
                {
                    agentState.Trace.Insert(0, new AgentTraceEntry() { ServerName = message.Server });
                    agentState.LastMessage = message;
                    continue;
                }

                if (message.ServerTag != agentState.LastMessage.ServerTag)
                {
                    if (message.Service.StartsWith("RETURN"))
                    {
                        var returnStr = message.Service.Split('_');
                        if (returnStr.Length != 2) throw new Exception("RETURN service should have 2 components - RETURN_value");
                        var returnValue = returnStr[1];

                        agentState.Trace.RemoveAt(0);
                        agentState.Trace[0].State = AgentTraceEntry.EntryState.Returned;
                        agentState.Trace[0].ReturnValue = returnValue;
                    }
                    else if (message.Service.StartsWith("TERMINATE"))
                    {
                        agentState.Trace.RemoveAt(0);
                        agentState.Trace[0].State = AgentTraceEntry.EntryState.Terminating;
                    }
                    else if (message.Service.StartsWith("CALL"))
                    {
                        var callStr = message.Service.Split('_');
                        if (callStr.Length != 4) throw new Exception("CALL service should have 4 components - CALL_ActionName_FROM_CallerName");
                        var actionName = callStr[1];

                        agentState.Trace[0].State = AgentTraceEntry.EntryState.At;
                        agentState.Trace.Insert(0, new AgentTraceEntry()
                        {
                            State = AgentTraceEntry.EntryState.Calling,
                            ServerName = message.Server,
                            CallingActionName = actionName
                        });
                    }
                }
                else
                {
                    if (message.Service.StartsWith("TERMINATE_EXIT"))
                    {
                        agentState.Trace.RemoveAt(0);
                    }
                    if (message.Service.StartsWith("EXEC"))
                    {
                        agentState.Trace[0].State = AgentTraceEntry.EntryState.Pre;
                        agentState.Trace[0].CodeLocation = CodeLocation.Parse(message.Service);
                    }
                    else if (message.Service.StartsWith("MISSING_CODE"))
                    {
                        agentState.Trace[0].State = AgentTraceEntry.EntryState.MissingCode;
                        agentState.Trace[0].CodeLocation = CodeLocation.Parse(message.Service);
                    }
                    else if (message.Service.StartsWith("PROCEED"))
                    {
                        agentState.Trace[0].State = AgentTraceEntry.EntryState.Post;
                        agentState.Trace[0].CodeLocation = CodeLocation.Parse(message.Service);
                    }
                }

                agentState.LastMessage = message;
                if (agentChanged != null)
                {
                    throw new Exception("More than one agent executed action during step!");
                }
                agentChanged = agentName;
            }

            return (serverChanged, agentChanged);
        }

        public IEnumerable<string> GetServerNames()
        {
            return _serverStates.Where(x => !x.Key.StartsWith("Process")).Select(x => x.Key);
        }

        public IReadOnlyDictionary<string, string> GetServerVariables(string serverName)
        {
            return ParseVariableStates(_serverStates[serverName]);
        }

        public IEnumerable<string> GetAgentNames()
        {
            return _agentTraces.Select(x => x.Key);
        }

        public IReadOnlyList<AgentTraceEntry> GetAgentTrace(string agentName)
        {
            return _agentTraces[agentName].Trace;
        }

        public string GetNextAgent()
        {
            if (_currentConfigurationIndex < 0 || !CanStep()) return null;

            int nextConfigurationIndex = _currentConfigurationIndex + 1;
            var nextTrailConfig = _trail.Configurations[nextConfigurationIndex];

            string nextAgent = null;

            foreach (var message in nextTrailConfig.Messages)
            {
                var agentName = GetIndexedName(message.Agent);
                var agentState = _agentTraces[agentName];
                if (CompareMessages(message, agentState.LastMessage)) continue;

                if (nextAgent != null)
                {
                    throw new Exception("More than one agent will execute action during next step!");
                }
                nextAgent = agentName;
            }

            return nextAgent;
        }

        private void FillReferences()
        {
            foreach (var conf in _trail.Configurations)
            {
                foreach (var action in conf.Actions)
                {
                    action.AgentReference = _trail.AgentVars.Single(x => x.AgentTag == action.AgentTag);
                    action.ServerReference = _trail.ServerVars.Single(x => x.ServerTag == action.ServerTag);
                    action.NextServerReference = action.NextServerTag != -1 ? _trail.ServerVars.Single(x => x.ServerTag == action.NextServerTag) : null;
                }
            }
        }

        private Dictionary<string, string> ParseVariableStates(string str)
        {
            var result = new Dictionary<string, string>();

            if (str.StartsWith("NONE")) return result;

            var endIndex = str.IndexOf("__AGENT");
            if (endIndex == -1) endIndex = str.IndexOf("_FROM");
            if (endIndex == -1) endIndex = str.IndexOf("_AT");
            if (endIndex != -1)
            {
                str = str.Substring(0, endIndex);
            }

            var split = str.Split('_');
            if (split.Length % 2 != 0) throw new ArgumentException("Incorrect format of str");
            for (int i = 0; i < split.Length - 1; i += 2)
            {
                string varName = GetIndexedName(split[i]);
                string varValue = split[i + 1];
                result.Add(varName, varValue);
            }

            return result;
        }

        private string GetIndexedName(string name)
        {
            if (name.Length > 4)
            {
                var realName = name.Substring(0, name.Length - 4);
                var trailingIndex = name.Substring(name.Length - 4, 4);
                if (int.TryParse(trailingIndex, out var index))
                {
                    return $"{realName}[{index}]";
                }
            }

            return name;
        }

        private bool CompareMessages(TrailSchema.Message a, TrailSchema.Message b)
        {
            return a?.AgentTag == b?.AgentTag &&
                a?.ServerTag == b?.ServerTag &&
                a?.Service == b?.Service;
        }

        public static Trail Parse(string input)
        {
            return (Trail)Serializer.Deserialize(new StringReader(input));
        }
    }
}
