using Rybu4WS.Language;
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

        public CodeLocation? CodeLocation { get; set; }

        /// <summary>
        /// Pending for external message
        /// </summary>
        public bool IsPending { get; set; }

        public List<Edge> OutEdges { get; set; } = new List<Edge>();

        public override string ToString()
        {
            var result = StatePair.ListToString(States);

            if (!string.IsNullOrEmpty(Caller))
            {
                result += $"_FROM_{Caller}";
            }
            if (CodeLocation != null)
            {
                result += $"_{(IsPending ? "AT" : "PRE")}_{CodeLocation}";
            }
            return result;
        }
    }
}
