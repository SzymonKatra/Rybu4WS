using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServCompEvol.Algorithm
{
    public class State
    {
        private List<Agent> _agents;

        public Node[] AgentStates { get; set; }

        public Node this[string agentName]
        {
            get => AgentStates[_agents.Single(x => x.Name == agentName).AgentIndex];
            set => AgentStates[_agents.Single(x => x.Name == agentName).AgentIndex] = value;
        }
        public Node this[Agent agent]
        {
            get => AgentStates[agent.AgentIndex];
            set => AgentStates[agent.AgentIndex] = value;
        }

        public State(List<Agent> agents)
        {
            _agents = agents;
            AgentStates = new Node[agents.Count];
        }

        public bool Contains(Node node)
        {
            return AgentStates.Any(x => x == node);
        }

        public Agent GetAgentAt(int index)
        {
            return _agents[index];
        }

        public State Clone()
        {
            var s = new State(_agents);
            for (int i = 0; i < AgentStates.Length; i++)
            {
                s.AgentStates[i] = AgentStates[i];
            }
            return s;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is State)) return false;

            return this == (State)obj;
        }

        public override int GetHashCode()
        {
            int result = 1;
            foreach (var item in AgentStates)
            {
                result *= item.GetHashCode();
            }
            return result;
        }

        public static bool operator ==(State a, State b)
        {
            if (a.AgentStates.Length != b.AgentStates.Length) return false;

            for (int i = 0; i < a.AgentStates.Length; i++)
            {
                if (a.AgentStates[i] != b.AgentStates[i]) return false;
            }

            return true;
        }

        public static bool operator !=(State a, State b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("{");
            for (int i = 0; i < AgentStates.Length; i++)
            {
                sb.Append(AgentStates[i].Name);
                if (i != AgentStates.Length - 1) sb.Append(", ");
            }
            sb.Append("}");

            return sb.ToString();
        }

        public double SimilarityTo(State x)
        {
            if (AgentStates.Length != x.AgentStates.Length) throw new Exception("Agent count different");

            int similiarities = 0;
            for (int i = 0; i < AgentStates.Length; i++)
            {
                if (AgentStates[i] == x.AgentStates[i]) similiarities++;
            }

            if (similiarities == AgentStates.Length) return 1;

            return (double)similiarities / (double)AgentStates.Length;
        }
    }
}
