using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServCompEvol.Algorithm
{
    public class Edge
    {
        public Node Source { get; set; }

        public Node Target { get; set; }

        public Agent[] AllowedAgents { get; set; }

        public Edge(Node source, Node target, Agent[] allowedAgents)
        {
            Source = source;
            Target = target;
            AllowedAgents = allowedAgents != null ? (Agent[])allowedAgents.Clone() : null;
        }

        public bool CanAgentUse(Agent agent)
        {
            return AllowedAgents == null || AllowedAgents.Contains(agent);
        }
    }
}
