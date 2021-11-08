using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.StateMachine
{
    public struct StatePair
    {
        public string Name;

        public string Value;

        public List<StatePair> Parse(string str)
        {
            var result = new List<StatePair>();

            var split = str.Split('_');
            if (split.Length % 2 != 0) throw new ArgumentException("Incorrect format of str");
            for (int i = 0; i < split.Length / 2; i += 2)
            {
                result.Add(new StatePair() { Name = split[i], Value = split[i + 1] });
            }

            return result;
        }
    }
}
