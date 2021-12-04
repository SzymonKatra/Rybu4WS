using System;
using System.Collections.Generic;
using System.Text;

namespace Rybu4WS.Language
{
    public class Group
    {
        public string Name { get; set; }

        public string ServerName => $"Group{Name}";

        public List<Variable> Variables { get; set; } = new List<Variable>();

        public List<Process> Processes { get; set; } = new List<Process>();
    }
}
