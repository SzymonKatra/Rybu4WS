using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.Language
{
    public abstract class BaseStatement : IWithCodeLocation
    {
        public CodeLocation CodeLocation { get; set; }

        public CodeLocation? PostCodeLocation { get; set; }
    }
}
