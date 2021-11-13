using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rybu4WS.Logic
{
    public struct CodeLocation
    {
        public int StartLine { get; set; }
        public int StartColumn { get; set; }

        public int EndLine { get; set; }
        public int EndColumn { get; set; }

        public override string ToString()
        {
            return $"SL{StartLine}SC{StartColumn}EL{EndLine}EC{EndColumn}";
        }

        public static CodeLocation Parse(string input)
        {
            var regex = new Regex("SL(?<startLine>[0-9]+)SC(?<startColumn>[0-9]+)EL(?<endLine>[0-9]+)EC(?<endColumn>[0-9]+)");
            var result = regex.Match(input);
            if (!result.Success) throw new ArgumentException("Invalid input");

            return new CodeLocation()
            {
                StartLine = int.Parse(result.Groups["startLine"].Value),
                StartColumn = int.Parse(result.Groups["startColumn"].Value),
                EndLine = int.Parse(result.Groups["endLine"].Value),
                EndColumn = int.Parse(result.Groups["endColumn"].Value),
            };
        }
    }
}
