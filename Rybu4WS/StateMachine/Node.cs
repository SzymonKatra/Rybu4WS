using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.StateMachine
{
    public class Node
    {
        public List<StatePair> States { get; set; }

        public string Caller { get; set; }

        public string CodeLocation { get; set; }

        /// <summary>
        /// Pending for external message
        /// </summary>
        public bool IsPending { get; set; }

        public List<Edge> OutEdges { get; set; }

        public override string ToString()
        {
            var result = string.Join('_', States.Select(x => $"{x.Name}_{x.Value}"));
            if (string.IsNullOrEmpty(result))
            {
                result = "NONE";
            }
            if (!string.IsNullOrEmpty(Caller))
            {
                result += $"_FROM_{Caller}";
            }
            if (!string.IsNullOrEmpty(CodeLocation))
            {
                result += $"_{(IsPending ? "AT" : "PRE")}_{CodeLocation}";
            }
            return result;
        }
    }
}
