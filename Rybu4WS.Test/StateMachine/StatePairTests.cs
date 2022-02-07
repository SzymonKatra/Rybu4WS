using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Rybu4WS.StateMachine;
using Xunit;

namespace Rybu4WS.Test.StateMachine
{
    public class StatePairTests
    {
        [Fact]
        public void Equality()
        {
            var a = new VariableValue("name", "value");
            var b = new VariableValue("name", "value");
            var c = new VariableValue("diff_name", "value");
            var d = new VariableValue("name", "diff_value");
            var e = new VariableValue("diff_name", "diff_value");

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
            var a = new VariableValue("name", "value");

            a.ToString().Should().Be("name_value");
        }

        [Fact]
        public void ListString()
        {
            var list = new List<VariableValue>() { new VariableValue("n1", "v1"), new VariableValue("n2", "v2"), new VariableValue("n3", "v3") };
            var listSingle = new List<VariableValue>() { new VariableValue("n1", "v1") };
            var listEmpty = new List<VariableValue>() { };

            VariableValue.ListToString(list).Should().Be("n1_v1_n2_v2_n3_v3");
            VariableValue.ListToString(listSingle).Should().Be("n1_v1");
            VariableValue.ListToString(listEmpty).Should().Be("NONE");
        }
    }
}
