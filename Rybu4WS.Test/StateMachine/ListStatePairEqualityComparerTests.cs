using FluentAssertions;
using Rybu4WS.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Rybu4WS.Test.StateMachine
{
    public class ListStatePairEqualityComparerTests
    {
        private ListVariableValueEqualityComparer _comparer;

        public ListStatePairEqualityComparerTests()
        {
            _comparer = new ListVariableValueEqualityComparer();
        }

        [Fact]
        public void ListEquality()
        {
            var a = new List<VariableValue>() { new VariableValue("a", "aval"), new VariableValue("b", "bval"), new VariableValue("c", "cval") };
            var b = new List<VariableValue>() { new VariableValue("a", "aval"), new VariableValue("b", "bval"), new VariableValue("c", "cval") };
            var c = new List<VariableValue>() { new VariableValue("a", "aval"), new VariableValue("b", "bval_x"), new VariableValue("c", "cval") };
            var d = new List<VariableValue>() { new VariableValue("a", "aval"), new VariableValue("c", "cval"), new VariableValue("b", "bval") };
            var e = new List<VariableValue>() { new VariableValue("a", "aval"), new VariableValue("b", "bval") };

            _comparer.Equals(a, b).Should().BeTrue();
            _comparer.Equals(a, c).Should().BeFalse();
            _comparer.Equals(a, d).Should().BeFalse();
            _comparer.Equals(a, e).Should().BeFalse();
        }
    }
}
