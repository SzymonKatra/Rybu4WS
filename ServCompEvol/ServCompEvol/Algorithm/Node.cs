using System;
using System.Collections.Generic;
using System.Text;

namespace ServCompEvol.Algorithm
{
    public class Node
    {
        public string Name { get; set; }

        public List<Edge> OutEdges { get; set; } = new List<Edge>();
        public List<Edge> InEdges { get; set; } = new List<Edge>();

        public bool IsTerminationNode => OutEdges.Count == 0;

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
