using System;
using System.Collections.Generic;
using System.Text;

namespace Rybu4WS.Language
{
    public class StatementLoop : BaseStatement
    {
        public List<BaseStatement> LoopStatements { get; set; } = new List<BaseStatement>();
    }
}
