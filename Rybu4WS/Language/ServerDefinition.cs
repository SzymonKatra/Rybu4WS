using System;
using System.Collections.Generic;
using System.Text;

namespace Rybu4WS.Language
{
    public class ServerDefinition : IWithCodeLocation
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public List<string> DependencyNameList { get; set; } = new List<string>();

        public Dictionary<string, string> VariablesInitialValues { get; set; } = new Dictionary<string, string>();

        public CodeLocation CodeLocation { get; set; }
    }
}
