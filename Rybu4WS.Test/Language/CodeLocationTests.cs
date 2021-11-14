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
                StartLine = 4,
                StartColumn = 7,
                EndLine = 34,
                EndColumn = 17
            };

            loc.ToString().Should().Be("SL4SC7EL34EC17");
        }

        [Fact]
        public void CodeLocation_Parse()
        {
            var loc = CodeLocation.Parse("SL4SC7EL34EC17");
            loc.Should().Be(new CodeLocation()
            {
                StartLine = 4,
                StartColumn = 7,
                EndLine = 34,
                EndColumn = 17
            });
        }
    }
}
