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

        public List<Edge> InEdges { get; set; } = new List<Edge>();

        public List<Edge> OutEdges { get; set; } = new List<Edge>();

        private CodeLocation? _postCodeLocation;
        public CodeLocation? PostCodeLocation
        {
            get
            {
                if (!_postCodeLocation.HasValue && CodeLocation.HasValue)
                {
                    return new CodeLocation()
                    {
                        StartIndex = CodeLocation.Value.EndIndex,
                        EndIndex = CodeLocation.Value.EndIndex,
                        StartLine = CodeLocation.Value.EndLine,
                        EndLine = CodeLocation.Value.EndLine,
                        StartColumn = CodeLocation.Value.EndColumn,
                        EndColumn = CodeLocation.Value.EndColumn
                    };
                }

                return _postCodeLocation;

            }
            set
            {
                _postCodeLocation = value;
            }
        }

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
