using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Rybu4WS.Test
{
    public class ListStatePairEqualityComparerTests
    {
        private ListStatePairEqualityComparer _comparer;

        public ListStatePairEqualityComparerTests()
        {
            _comparer = new ListStatePairEqualityComparer();
        }

        [Fact]
        public void ListEquality()
        {
            var a = new List<StatePair>() { new StatePair("a", "aval"), new StatePair("b", "bval"), new StatePair("c", "cval") };
            var b = new List<StatePair>() { new StatePair("a", "aval"), new StatePair("b", "bval"), new StatePair("c", "cval") };
            var c = new List<StatePair>() { new StatePair("a", "aval"), new StatePair("b", "bval_x"), new StatePair("c", "cval") };
            var d = new List<StatePair>() { new StatePair("a", "aval"), new StatePair("c", "cval"), new StatePair("b", "bval") };
            var e = new List<StatePair>() { new StatePair("a", "aval"), new StatePair("b", "bval") };

            _comparer.Equals(a, b).Should().BeTrue();
            _comparer.Equals(a, c).Should().BeFalse();
            _comparer.Equals(a, d).Should().BeFalse();
            _comparer.Equals(a, e).Should().BeFalse();
        }
    }
}
