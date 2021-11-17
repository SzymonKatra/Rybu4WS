using FluentAssertions;
using Rybu4WS.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Rybu4WS.Test.Logic
{
    public class CodeLocationTests
    {
        [Fact]
        public void CodeLocation_ToString()
        {
            var loc = new CodeLocation()
            {
                StartIndex = 30,
                EndIndex = 50,
                StartLine = 4,
                StartColumn = 7,
                EndLine = 34,
                EndColumn = 17
            };

            loc.ToString().Should().Be("SI30EI50SL4SC7EL34EC17");
        }

        [Fact]
        public void CodeLocation_Parse()
        {
            var loc = CodeLocation.Parse("SI30EI50SL4SC7EL34EC17");
            loc.Should().Be(new CodeLocation()
            {
                StartIndex = 30,
                EndIndex = 50,
                StartLine = 4,
                StartColumn = 7,
                EndLine = 34,
                EndColumn = 17
            });
        }

        [Fact]
        public void CodeLocation_Parse_WithOtherInfo()
        {
            var loc = CodeLocation.Parse("ENTER_PRE_SI30EI50SL4SC7EL34EC17_FROM_Caller");
            loc.Should().Be(new CodeLocation()
            {
                StartIndex = 30,
                EndIndex = 50,
                StartLine = 4,
                StartColumn = 7,
                EndLine = 34,
                EndColumn = 17
            });
        }
    }
}
