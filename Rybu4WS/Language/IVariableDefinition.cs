using System;
using System.Collections.Generic;
using System.Text;

namespace Rybu4WS.Language
{
    public interface IVariableDefinition
    {
        VariableType Type { get; set; }

        List<string> AvailableValues { get; set; }
    }
}
