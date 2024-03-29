﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.Language
{
    public class ConditionLeaf : ICondition, IWithCodeLocation
    {
        public string VariableName { get; set; }

        public ConditionOperator Operator { get; set; }

        public string Value { get; set; }

        public VariableType VariableType { get; set; }

        public CodeLocation CodeLocation { get; set; }

        public ICondition Clone()
        {
            return new ConditionLeaf()
            {
                VariableName = this.VariableName,
                Operator = this.Operator,
                Value = this.Value,
                VariableType = this.VariableType,
                CodeLocation = this.CodeLocation
            };
        }
    }
}
