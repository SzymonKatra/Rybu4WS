using Rybu4WS.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rybu4WS.StateMachine.Composed
{
    public class ComposedState
    {
        public class AgentState
        {
            public string Caller { get; set; }

            public CodeLocation? CodeLocation { get; set; }

            /// <summary>
            /// Pending for external message
            /// </summary>
            public bool IsPending { get; set; }

            public List<ComposedAction> InEdges { get; set; } = new List<ComposedAction>();

            public List<ComposedAction> OutEdges { get; set; } = new List<ComposedAction>();

            public State BaseNodeReference { get; set; }

            public override string ToString()
            {
                var result = "";
                if (!string.IsNullOrEmpty(Caller))
                {
                    result += $"FROM_{Caller}";
                }
                if (CodeLocation != null)
                {
                    result += $"_{(IsPending ? "AT" : "PRE")}_{CodeLocation}";
                }
                return result;
            }
        }

        public List<VariableValue> States { get; set; }

        public Dictionary<int, AgentState> Agents { get; set; } = new Dictionary<int, AgentState>();

        public override string ToString()
        {
            var result = VariableValue.ListToString(States);

            var agentStrings = Agents.Where(x => x.Value.ToString().Length > 0).Select(x => $"AGENT{x.Key}_{x.Value}");

            return string.Join("__", (new [] { result }).Concat(agentStrings));
        }
    }
}
