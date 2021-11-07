using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.Logic
{
    public class StatementStateMutation : BaseStatement
    {
        public string VariableName { get; set; }

        public StateMutationOperator Operator { get; set; }

        public string Value { get; set; }
    }
}
