using System;
using System.Collections.Generic;
using System.Text;

namespace ServCompEvol.Algorithm
{
    public class Agent
    {
        public int AgentIndex { get; set; }

        public string Name { get; set; }

        public Agent(string name, int index)
        {
            Name = name;
            AgentIndex = index;
        }
    }
}
