using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Rybu4WS.Test
{
    public class StatePairTests
    {
        [Fact]
        public void Equality()
        {
            var a = new StatePair("name", "value");
            var b = new StatePair("name", "value");
            var c = new StatePair("diff_name", "value");
            var d = new StatePair("name", "diff_value");
            var e = new StatePair("diff_name", "diff_value");

            (a == b).Should().BeTrue();
            (a == c).Should().BeFalse();
            (a == d).Should().BeFalse();
            (a == e).Should().BeFalse();

            (a != b).Should().BeFalse();
            (a != c).Should().BeTrue();
            (a != d).Should().BeTrue();
            (a != e).Should().BeTrue();
        }

        [Fact]
        public void String()
        {
            var a = new StatePair("name", "value");

            a.ToString().Should().Be("name_value");
        }

        [Fact]
        public void ListString()
        {
            var list = new List<StatePair>() { new StatePair("n1", "v1"), new StatePair("n2", "v2"), new StatePair("n3", "v3") };
            var listSingle = new List<StatePair>() { new StatePair("n1", "v1") };
            var listEmpty = new List<StatePair>() { };

            StatePair.ListToString(list).Should().Be("n1_v1_n2_v2_n3_v3");
            StatePair.ListToString(listSingle).Should().Be("n1_v1");
            StatePair.ListToString(listEmpty).Should().Be("");
        }
    }
}
