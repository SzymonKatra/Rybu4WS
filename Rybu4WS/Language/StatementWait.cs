using System;
using System.Collections.Generic;
using System.Text;

namespace Rybu4WS.Language
{
    public class StatementWait : BaseStatement
    {
        public ICondition Condition { get; set; }
    }
}
