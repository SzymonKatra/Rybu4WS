using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServCompEvol
{
    class Edge
    {
        public Node Source { get; set; }

        public Node Target { get; set; }

        public int? OnlyForAgent { get; set; }

        public Edge(Node source, Node target, int? onlyForAgent = null)
        {
            Source = source;
            Target = target;
            OnlyForAgent = onlyForAgent;
        }

        public bool CanAgentUse(int agentIndex)
        {
            return OnlyForAgent == null || OnlyForAgent == agentIndex;
        }
    }

    class Node
    {
        public string Name { get; set; }

        public List<Edge> OutEdges { get; set; } = new List<Edge>();
        public List<Edge> InEdges { get; set; } = new List<Edge>();

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }

    class Graph
    {
        public List<Node> Nodes { get; set; } = new List<Node>();
        public List<Edge> Edges { get; set; } = new List<Edge>();

        public Node this[string nodeName]
        {
            get => Nodes.Single(x => x.Name == nodeName);
        }

        public void AddNode(string nodeName)
        {
            Nodes.Add(new Node() { Name = nodeName });
        }

        public void AddEdge(string sourceNodeName, string targetNodeName, int? onlyForAgent = null)
        {
            var src = Nodes.Single(x => x.Name == sourceNodeName);
            var dst = Nodes.Single(x => x.Name == targetNodeName);

            var edge = new Edge(src, dst, onlyForAgent);
            Edges.Add(edge);

            src.OutEdges.Add(edge);
            dst.InEdges.Add(edge);
        }
    }

    class State
    {
        public Node[] Agent { get; set; }

        public State(int agentCount)
        {
            Agent = new Node[agentCount];
        }

        public bool Contains(Node node)
        {
            return Agent.Any(x => x == node);
        }

        public State Clone()
        {
            var s = new State(Agent.Length);
            for (int i = 0; i < Agent.Length; i++)
            {
                s.Agent[i] = Agent[i];
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
            foreach (var item in Agent)
            {
                result *= item.GetHashCode();
            }
            return result;
        }

        public static bool operator==(State a, State b)
        {
            if (a.Agent.Length != b.Agent.Length) return false;

            for (int i = 0; i < a.Agent.Length; i++)
            {
                if (a.Agent[i] != b.Agent[i]) return false;
            }

            return true;
        }

        public static bool operator!=(State a, State b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("{");
            for (int i = 0; i < Agent.Length; i++)
            {
                sb.Append(Agent[i].Name);
                if (i != Agent.Length - 1) sb.Append(", ");
            }
            sb.Append("}");

            return sb.ToString();
        }

        public double SimilarityTo(State x)
        {
            if (Agent.Length != x.Agent.Length) throw new Exception("Agent count different");

            int similiarities = 0;
            for (int i = 0; i < Agent.Length; i++)
            {
                if (Agent[i] == x.Agent[i]) similiarities++;
            }

            if (similiarities == Agent.Length) return 1;

            return (double)similiarities / (double)Agent.Length;
        }
    }

    class Path
    {
        public List<State> States;

        public Path(List<State> states)
        {
            States = states;
        }

        public void RemoveLoops()
        {
            var firstNodeIndex = States.FindIndex(x => States.Any(y => x == y && !object.ReferenceEquals(x, y)));

            while (firstNodeIndex != -1)
            {
                var lastNodeIndex = States.FindLastIndex(x => x == States[firstNodeIndex]);

                States.RemoveRange(firstNodeIndex + 1, lastNodeIndex - firstNodeIndex);
                firstNodeIndex = States.FindIndex(x => States.Any(y => x == y && !object.ReferenceEquals(x, y)));
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var item in States)
            {
                sb.AppendLine(item.ToString());
            }
            return sb.ToString();
        }

        public static IEnumerable<Path> CombineOn(Path a, Path b, State s)
        {
            var aIndex = a.States.IndexOf(s);
            var bIndex = b.States.IndexOf(s);

            var first = new List<State>();
            for (int i = 0; i <= aIndex; i++)
            {
                first.Add(a.States[i].Clone());
            }
            for (int i = bIndex + 1; i < b.States.Count; i++)
            {
                first.Add(b.States[i].Clone());
            }
            var firstPath = new Path(first);
            firstPath.RemoveLoops();
            yield return firstPath;

            var second = new List<State>();
            for (int i = 0; i < bIndex; i++)
            {
                second.Add(b.States[i].Clone());
            }
            for (int i = aIndex + 1; i < a.States.Count; i++)
            {
                second.Add(a.States[i].Clone());
            }
            var secondPath = new Path(second);
            secondPath.RemoveLoops();
            yield return secondPath;
        }

        public bool HaveSameStartAndEnd(Path p)
        {
            return States.First() == p.States.First() && States.Last() == p.States.Last();
        }

        public bool IsSameAs(Path p)
        {
            if (States.Count != p.States.Count) return false;

            for (int i = 0; i < States.Count; i++)
            {
                if (States[i] != p.States[i]) return false;
            }

            return true;
        }
    }

    class Program
    {
        static Random rnd = new Random();
        static readonly int AgentCount = 3;
        static readonly int GenCount = 50;
        static readonly int TakePopulationFromPrev = 30;
        static readonly int NewPopulation = 30;
        static readonly int PathLen = 20;


        static void Main(string[] args)
        {
            Graph g = new Graph();
            g.AddNode("1");
            g.AddNode("2");
            g.AddNode("3");
            g.AddNode("4");
            g.AddNode("5");
            g.AddNode("6");
            g.AddNode("7");
            g.AddNode("S1");
            g.AddNode("S2");
            g.AddNode("S3");

            g.AddEdge("1", "S3", 2);
            g.AddEdge("1", "2");
            g.AddEdge("1", "3");

            g.AddEdge("2", "S2", 1);
            g.AddEdge("2", "1");

            g.AddEdge("3", "1");
            g.AddEdge("3", "4");
            g.AddEdge("3", "5");

            g.AddEdge("4", "3");

            g.AddEdge("5", "7");
            g.AddEdge("5", "3");
            g.AddEdge("5", "6");

            g.AddEdge("6", "S1", 0);
            g.AddEdge("6", "5");

            g.AddEdge("7", "5");

            //Graph ig = g.CloneInvert();

            //Path p = new Path(new List<State>()
            //{
            //    new State(AgentCount) { Agent = new Node[] { g["1"], g["2"], g["3"] } },
            //    new State(AgentCount) { Agent = new Node[] { g["2"], g["2"], g["3"] } },
            //    new State(AgentCount) { Agent = new Node[] { g["3"], g["2"], g["3"] } },
            //    new State(AgentCount) { Agent = new Node[] { g["4"], g["2"], g["3"] } },
            //    new State(AgentCount) { Agent = new Node[] { g["5"], g["2"], g["3"] } },
            //    new State(AgentCount) { Agent = new Node[] { g["6"], g["2"], g["3"] } },
            //    new State(AgentCount) { Agent = new Node[] { g["3"], g["2"], g["3"] } },
            //    new State(AgentCount) { Agent = new Node[] { g["S1"], g["2"], g["3"] } },
            //    new State(AgentCount) { Agent = new Node[] { g["S2"], g["2"], g["3"] } },
            //    new State(AgentCount) { Agent = new Node[] { g["S3"], g["2"], g["3"] } },
            //});
            //Console.WriteLine(p);
            //p.RemoveLoops();
            //Console.WriteLine(p);

            State stateBegin = new State(AgentCount);
            stateBegin.Agent[0] = g["2"];
            stateBegin.Agent[1] = g["6"];
            stateBegin.Agent[2] = g["7"];

            State stateEnd = new State(AgentCount);
            stateEnd.Agent[0] = g["S1"];
            stateEnd.Agent[1] = g["S2"];
            stateEnd.Agent[2] = g["S3"];

            var possibleNodes = g.Nodes.Where(x => !x.Name.StartsWith("S")).ToList();
            var population = new List<Path>();
            var mixHelper = new Dictionary<State, List<Path>>();

            for (int i = 0; i < TakePopulationFromPrev / 2; i++)
            {
                var path = BuildPath(stateBegin, PathLen);
                population.Add(path);
            }

            for (int i = 0; i < TakePopulationFromPrev / 2; i++)
            {
                var path = BuildReversePath(stateEnd, PathLen);
                population.Add(path);
            }

            for (int i = 0; i < NewPopulation; i++)
            {
                var state = GenerateRandomState(possibleNodes, AgentCount);
                var path = BuildPath(state, PathLen);
                population.Add(path);
                //Console.WriteLine(path.ToString());
            }
           
            for (int gen = 0; gen < GenCount; gen++)
            {
                mixHelper.Clear();
                foreach (var item in population)
                {
                    ReversePathLookup(mixHelper, item);
                }
                mixHelper = mixHelper
                   .Where(x => x.Value.Count > 1)
                   //.OrderBy(x => x.Value.Count)
                   //.Take(10)
                   .ToDictionary(x => x.Key, x => x.Value);

                var mixes = new List<Path>();
                foreach (var item in mixHelper)
                {
                    var tmpPaths = item.Value.OrderByDescending(x => x.States.Count).Take(2).ToList();
                    mixes.AddRange(Path.CombineOn(tmpPaths[0], tmpPaths[1], item.Key));
                }

                var mixesAndPopulation = mixes.Concat(population)
                    .Where(x => x.States.Count > 1)
                    .ToList();

                // exactly the same
                var samePathIndex = mixesAndPopulation.FindIndex(x => mixesAndPopulation.Any(y => !object.ReferenceEquals(x, y) && x.IsSameAs(y)));
                while (samePathIndex != -1)
                {
                    var samePath = mixesAndPopulation[samePathIndex];
                    var secSamePathIndex = mixesAndPopulation.FindLastIndex(x => x.IsSameAs(samePath));
                    mixesAndPopulation.RemoveAt(secSamePathIndex);
                    samePathIndex = mixesAndPopulation.FindIndex(x => mixesAndPopulation.Any(y => !object.ReferenceEquals(x, y) && x.IsSameAs(y)));
                }

                // start and end the same
                samePathIndex = mixesAndPopulation.FindIndex(x => mixesAndPopulation.Any(y => !object.ReferenceEquals(x, y) && x.HaveSameStartAndEnd(y)));
                while (samePathIndex != -1)
                {
                    var samePath = mixesAndPopulation[samePathIndex];
                    var secSamePathIndex = mixesAndPopulation.FindLastIndex(x => x.HaveSameStartAndEnd(samePath));
                    mixesAndPopulation.RemoveAt(secSamePathIndex);
                    samePathIndex = mixesAndPopulation.FindIndex(x => mixesAndPopulation.Any(y => !object.ReferenceEquals(x, y) && x.HaveSameStartAndEnd(y)));
                }

                var similarToStart = mixesAndPopulation
                    .OrderByDescending(x => x.States.First().SimilarityTo(stateBegin))
                    .ThenByDescending(x => x.States.Count)
                    .Take(TakePopulationFromPrev / 2)
                    .ToList();

                var similarToEnd = mixesAndPopulation
                    .OrderByDescending(x => x.States.Last().SimilarityTo(stateEnd))
                    .ThenByDescending(x => x.States.Count)
                    .Take(TakePopulationFromPrev / 2)
                    .ToList();

                population.Clear();
                population.AddRange(similarToStart);
                population.AddRange(similarToEnd);

                var haveOkStart = similarToStart.Where(x => x.States.First().SimilarityTo(stateBegin) == 1).Take(3).ToList();
                var haveOkEnd = similarToEnd.Where(x => x.States.Last().SimilarityTo(stateEnd) == 1).Take(3).ToList();

                foreach (var item in haveOkStart)
                {
                    population.Add(BuildPath(item.States.Last(), PathLen));

                }
                foreach (var item in haveOkEnd)
                {
                    population.Add(BuildReversePath(item.States.First(), PathLen));
                }

                for (int i = 0; i < NewPopulation / 2; i++)
                {
                    var state = GenerateRandomState(possibleNodes, AgentCount);
                    population.Add(BuildPath(state, PathLen));
                    population.Add(BuildReversePath(state, PathLen));
                }

                var result = population.Find(x => x.States.First() == stateBegin && x.States.Last() == stateEnd);
                if (result != null)
                {
                    Console.WriteLine($"Generation = {gen}");
                    Console.WriteLine(result);
                    break;
                }
            }
        }

        static void ReversePathLookup(Dictionary<State, List<Path>> dict, Path path)
        {
            foreach (var item in path.States)
            {
                if (dict.ContainsKey(item))
                {
                    dict[item].Add(path);
                }
                else
                {
                    dict.Add(item, new List<Path> { path });
                }
            }
        }

        static State GenerateRandomState(List<Node> possibleNodes, int agentCount)
        {
            var state = new State(agentCount);

            for (int i = 0; i < agentCount; i++)
            {
                int nodeIndex = rnd.Next(0, possibleNodes.Count);
                int originalNodeIndex = nodeIndex;
                var node = possibleNodes[nodeIndex];
                while(state.Contains(node))
                {
                    nodeIndex = (nodeIndex + 1) % possibleNodes.Count;
                    node = possibleNodes[nodeIndex];
                    if (nodeIndex == originalNodeIndex) throw new Exception("Cannot generate unique random state");
                }
                state.Agent[i] = node;
            }

            return state;
        }

        static Path BuildPath(State start, int depth)
        {
            IEnumerable<State> generate()
            {
                List<State> states = new List<State>();

                int remaining = depth;

                var current = start.Clone();
                states.Add(current);
                remaining--;

                while (remaining-- > 0)
                {
                    var next = current.Clone();

                    var agentIndex = rnd.Next(0, next.Agent.Length);
                    var originalAgentIndex = agentIndex;
                    while (next.Agent[agentIndex].OutEdges.Count == 0)
                    {
                        agentIndex = (agentIndex + 1) % next.Agent.Length;
                        if (agentIndex == originalAgentIndex) return states;
                    }

                    var outEdgeIndex = rnd.Next(0, next.Agent[agentIndex].OutEdges.Count);
                    var originalOutEdgeIndex = outEdgeIndex;
                    var edge = next.Agent[agentIndex].OutEdges[outEdgeIndex];
                    while (next.Contains(edge.Target) || !edge.CanAgentUse(agentIndex))
                    {
                        outEdgeIndex = (outEdgeIndex + 1) % next.Agent[agentIndex].OutEdges.Count;
                        if (outEdgeIndex == originalOutEdgeIndex) return states;
                        edge = next.Agent[agentIndex].OutEdges[outEdgeIndex];
                    }

                    next.Agent[agentIndex] = edge.Target;

                    if (states.Contains(next)) return states; // detected loop

                    states.Add(next);
                    current = next;
                }

                return states;
            }

            return new Path(generate().ToList());
        }

        static Path BuildReversePath(State end, int depth)
        {
            IEnumerable<State> generate()
            {
                List<State> states = new List<State>();

                int remaining = depth;

                var current = end.Clone();
                states.Add(current);
                remaining--;

                while (remaining-- > 0)
                {
                    var next = current.Clone();

                    var agentIndex = rnd.Next(0, next.Agent.Length);
                    var originalAgentIndex = agentIndex;
                    while (next.Agent[agentIndex].InEdges.Count == 0)
                    {
                        agentIndex = (agentIndex + 1) % next.Agent.Length;
                        if (agentIndex == originalAgentIndex) return states;
                    }

                    var inEdgeIndex = rnd.Next(0, next.Agent[agentIndex].InEdges.Count);
                    var originalInEdgeIndex = inEdgeIndex;
                    var edge = next.Agent[agentIndex].InEdges[inEdgeIndex];
                    while (next.Contains(edge.Source) || !edge.CanAgentUse(agentIndex))
                    {
                        inEdgeIndex = (inEdgeIndex + 1) % next.Agent[agentIndex].InEdges.Count;
                        if (inEdgeIndex == originalInEdgeIndex) return states;
                        edge = next.Agent[agentIndex].InEdges[inEdgeIndex];
                    }

                    next.Agent[agentIndex] = edge.Source;

                    if (states.Contains(next)) return states; // detected loop

                    states.Add(next);
                    current = next;
                }

                return states;
            }

            return new Path(generate().Reverse().ToList());
        }
    }
}
