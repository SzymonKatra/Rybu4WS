using System;
using System.Collections.Generic;
using System.Text;

namespace Rybu4WS.Language
{
    public class TimedDelay : IWithCodeLocation
    {
        public bool IsLeftInclusive { get; set; }

        public int LeftValue { get; set; }

        public bool IsRightInclusive { get; set; }

        public int RightValue { get; set; }

        public CodeLocation CodeLocation { get; set; }

        public TimedDelay Clone()
        {
            return new TimedDelay()
            {
                IsLeftInclusive = this.IsLeftInclusive,
                LeftValue = this.LeftValue,
                IsRightInclusive = this.IsRightInclusive,
                RightValue = this.RightValue,
                CodeLocation = this.CodeLocation
            };
        }

        public string ToDedan()
        {
            string value = LeftValue == RightValue ? $"{LeftValue}" : $"{LeftValue}, {RightValue}";
            return $"{(IsLeftInclusive ? '<' : '(')}{value}{(IsRightInclusive ? '>' : ')')}";
        }
    }
}
