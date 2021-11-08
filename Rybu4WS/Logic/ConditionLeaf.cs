using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.Logic
{
    public class ConditionLeaf : ICondition
    {
        public string VariableName { get; set; }

        public ConditionOperator Operator { get; set; }

        public string Value { get; set; }

        public VariableType VariableType { get; set; }
    }
}
