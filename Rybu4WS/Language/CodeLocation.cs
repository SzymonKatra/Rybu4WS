using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rybu4WS.Language
{
    public struct CodeLocation
    {
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }

        public int StartLine { get; set; }
        public int StartColumn { get; set; }

        public int EndLine { get; set; }
        public int EndColumn { get; set; }

        public override string ToString()
        {
            return $"SI{StartIndex}EI{EndIndex}SL{StartLine}SC{StartColumn}EL{EndLine}EC{EndColumn}";
        }

        public static CodeLocation Parse(string input)
        {
            var regex = new Regex("SI(?<startIndex>[0-9]+)EI(?<endIndex>[0-9]+)SL(?<startLine>[0-9]+)SC(?<startColumn>[0-9]+)EL(?<endLine>[0-9]+)EC(?<endColumn>[0-9]+)");
            var result = regex.Match(input);
            if (!result.Success) throw new ArgumentException("Invalid input");

            return new CodeLocation()
            {
                StartIndex = int.Parse(result.Groups["startIndex"].Value),
                EndIndex = int.Parse(result.Groups["endIndex"].Value),
                StartLine = int.Parse(result.Groups["startLine"].Value),
                StartColumn = int.Parse(result.Groups["startColumn"].Value),
                EndLine = int.Parse(result.Groups["endLine"].Value),
                EndColumn = int.Parse(result.Groups["endColumn"].Value),
            };
        }

        public static bool operator ==(CodeLocation x, CodeLocation y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(CodeLocation x, CodeLocation y)
        {
            return !x.Equals(y);
        }

        public override bool Equals(object obj)
        {
            if (obj is CodeLocation)
            {
                var cl = (CodeLocation)obj;
                return this.StartIndex == cl.StartIndex && this.EndIndex == cl.EndIndex &&
                    this.StartLine == cl.StartLine && this.EndLine == cl.EndLine &&
                    this.StartColumn == cl.StartColumn && this.EndColumn == cl.EndColumn;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(StartIndex, EndIndex, StartLine, StartColumn, EndLine, EndColumn);
        }
    }
}
