using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.Language
{
    public class StatementMatchOption : IWithCodeLocation
    {
        public string HandledValue { get; set; }

        public List<BaseStatement> HandlerStatements { get; set; } = new List<BaseStatement>();

        public CodeLocation CodeLocation { get; set; }
    }
}
