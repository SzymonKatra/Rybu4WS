using System;
using System.Collections.Generic;
using System.Text;

namespace Rybu4WS.Language
{
    public class StatementIf : BaseStatement
    {
        public ICondition Condition { get; set; }

        public List<BaseStatement> ConditionStatements { get; set; } = new List<BaseStatement>();
    }
}
