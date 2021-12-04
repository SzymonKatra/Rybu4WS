using System;
using System.Collections.Generic;
using System.Text;

namespace Rybu4WS.Language
{
    public class ConditionNot : ICondition
    {
        public ICondition Condition { get; set; }

        public ICondition Clone()
        {
            return new ConditionNot()
            {
                Condition = this.Condition.Clone()
            };
        }
    }
}
