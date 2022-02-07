using Rybu4WS.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.StateMachine
{
    public struct VariableValue
    {
        public string Name;

        public string Value;

        public Variable VariableReference;

        public VariableValue(string name, string value)
        {
            Name = name;
            Value = value;
            VariableReference = null;
        }

        public VariableValue(string name, string value, Variable variableReference)
        {
            Name = name;
            Value = value;
            VariableReference = variableReference;
        }

        public VariableValue(Variable variableReference)
        {
            Name = variableReference.Name;
            Value = variableReference.InitialValue;
            VariableReference = variableReference;
        }

        public List<VariableValue> Parse(string str)
        {
            var result = new List<VariableValue>();

            var split = str.Split('_');
            if (split.Length % 2 != 0) throw new ArgumentException("Incorrect format of str");
            for (int i = 0; i < split.Length / 2; i += 2)
            {
                result.Add(new VariableValue() { Name = split[i], Value = split[i + 1] });
            }

            return result;
        }

        public static bool operator==(VariableValue x, VariableValue y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(VariableValue x, VariableValue y)
        {
            return !x.Equals(y);
        }

        public override bool Equals(object obj)
        {
            if (obj is VariableValue)
            {
                var sp = (VariableValue)obj;
                return this.Name == sp.Name && this.Value == sp.Value;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() + Value.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Name}_{Value}";
        }

        public static string ListToString(List<VariableValue> list)
        {
            if (list.Count == 0) return "NONE";

            return string.Join('_', list.Select(x => x.ToString()));
        }
    }
}
