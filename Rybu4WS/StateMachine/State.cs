using Rybu4WS.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.StateMachine
{
    public class State
    {
        public List<VariableValue> VariableValues { get; set; }

        public string Caller { get; set; }

        public CodeLocation? CodeLocation { get; set; }

        /// <summary>
        /// Pending for external message
        /// </summary>
        public bool IsPending { get; set; }

        public List<Action> InActions { get; set; } = new List<Action>();

        public List<Action> OutActions { get; set; } = new List<Action>();

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
            var result = VariableValue.ListToString(VariableValues);

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
