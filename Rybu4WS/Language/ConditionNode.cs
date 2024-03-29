﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.Language
{
    public class ConditionNode : ICondition
    {
        public ICondition Left { get; set; }

        public ConditionLogicalOperator Operator { get; set; }

        public ICondition Right { get; set; }

        public ICondition Clone()
        {
            return new ConditionNode()
            {
                Left = this.Left.Clone(),
                Operator = this.Operator,
                Right = this.Right.Clone()
            };
        }
    }
}
