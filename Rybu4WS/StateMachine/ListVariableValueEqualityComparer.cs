using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.StateMachine
{
    public class ListVariableValueEqualityComparer : IEqualityComparer<List<VariableValue>>
    {
        public bool Equals(List<VariableValue> x, List<VariableValue> y)
        {
            if (x.Count != y.Count) return false;

            for (int i = 0; i < x.Count; i++)
            {
                var a = x[i];
                var b = y[i];
                if (a != b) return false;
            }

            return true;
        }

        public int GetHashCode([DisallowNull] List<VariableValue> obj)
        {
            var result = 0;
            foreach (var item in obj)
            {
                result += item.GetHashCode();
            }
            return result;
        }
    }
}
