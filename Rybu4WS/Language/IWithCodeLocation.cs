using System;
using System.Collections.Generic;
using System.Text;

namespace Rybu4WS.Language
{
    public interface IWithCodeLocation
    {
        CodeLocation CodeLocation { get; set; }
    }
}
