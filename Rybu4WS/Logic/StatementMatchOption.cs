using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.Logic
{
    public class StatementMatchOption
    {
        public string HandledValue { get; set; }

        public List<BaseStatement> HandlerStatements { get; set; } = new List<BaseStatement>();
    }
}
