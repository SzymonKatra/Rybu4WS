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

        public int[] OnlyForAgents { get; set; }

        public Edge(Node source, Node target, int? onlyForAgent = null)
        {
            Source = source;
            Target = target;
            if (onlyForAgent.HasValue)
            {
                OnlyForAgents = new int[] { onlyForAgent.Value };
            }
        }

        public Edge(Node source, Node target, int[] onlyForAgents)
        {
            Source = source;
            Target = target;
            OnlyForAgents = (int[])onlyForAgents.Clone();
        }

        public bool CanAgentUse(int agentIndex)
        {
            return OnlyForAgents == null || OnlyForAgents.Contains(agentIndex);
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

        public void AddEdge(string sourceNodeName, string targetNodeName, int[] onlyForAgents)
        {
            var src = Nodes.Single(x => x.Name == sourceNodeName);
            var dst = Nodes.Single(x => x.Name == targetNodeName);

            var edge = new Edge(src, dst, onlyForAgents);
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
            for (int i = 0; i <= bIndex; i++)
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
        static readonly int AgentCount = 5;
        static readonly int GenCount = 50000;
        static readonly int TakePopulationFromPrev = 45;
        static readonly int NewPopulationRandom = 10;
        static readonly int PathLen = 20;
        static readonly int NewPopulationSimilar = 20;


        static void Main(string[] args)
        {
            // 3 agents

            //Graph g = new Graph();
            //g.AddNode("1");
            //g.AddNode("2");
            //g.AddNode("3");
            //g.AddNode("4");
            //g.AddNode("5");
            //g.AddNode("6");
            //g.AddNode("7");
            //g.AddNode("S1");
            //g.AddNode("S2");
            //g.AddNode("S3");

            //g.AddEdge("1", "S3", 2);
            //g.AddEdge("1", "2");
            //g.AddEdge("1", "3");

            //g.AddEdge("2", "S2", 1);
            //g.AddEdge("2", "1");

            //g.AddEdge("3", "1");
            //g.AddEdge("3", "4");
            //g.AddEdge("3", "5");

            //g.AddEdge("4", "3");

            //g.AddEdge("5", "7");
            //g.AddEdge("5", "3");
            //g.AddEdge("5", "6");

            //g.AddEdge("6", "S1", 0);
            //g.AddEdge("6", "5");

            //g.AddEdge("7", "5");

            //State stateBegin = new State(AgentCount);
            //stateBegin.Agent[0] = g["2"];
            //stateBegin.Agent[1] = g["6"];
            //stateBegin.Agent[2] = g["7"];

            //State stateEnd = new State(AgentCount);
            //stateEnd.Agent[0] = g["S1"];
            //stateEnd.Agent[1] = g["S2"];
            //stateEnd.Agent[2] = g["S3"];

            // ----------------

            //// 4 agents
            //Graph g = new Graph();
            //g.AddNode("1");
            //g.AddNode("2");
            //g.AddNode("3");
            //g.AddNode("4");
            //g.AddNode("5");
            //g.AddNode("6");
            //g.AddNode("7");
            //g.AddNode("8");
            //g.AddNode("S1");
            //g.AddNode("S2");
            //g.AddNode("S3");
            //g.AddNode("S4");

            //g.AddEdge("1", "S3", 2);
            //g.AddEdge("1", "2");
            //g.AddEdge("1", "3");
            //g.AddEdge("1", "8");

            //g.AddEdge("2", "S2", 1);
            //g.AddEdge("2", "1");

            //g.AddEdge("3", "1");
            //g.AddEdge("3", "4");
            //g.AddEdge("3", "5");

            //g.AddEdge("4", "3");
            //g.AddEdge("4", "S4", 3);

            //g.AddEdge("5", "7");
            //g.AddEdge("5", "3");
            //g.AddEdge("5", "6");

            //g.AddEdge("6", "S1", 0);
            //g.AddEdge("6", "5");

            //g.AddEdge("7", "5");

            //g.AddEdge("8", "1");


            //State stateBegin = new State(AgentCount);
            //stateBegin.Agent[0] = g["2"];
            //stateBegin.Agent[1] = g["6"];
            //stateBegin.Agent[2] = g["7"];
            //stateBegin.Agent[3] = g["8"];

            //State stateEnd = new State(AgentCount);
            //stateEnd.Agent[0] = g["S1"];
            //stateEnd.Agent[1] = g["S2"];
            //stateEnd.Agent[2] = g["S3"];
            //stateEnd.Agent[3] = g["S4"];
            //// -----------------------

            // ----------------

            //// 5 agents
            //Graph g = new Graph();
            //g.AddNode("1");
            //g.AddNode("2");
            //g.AddNode("3");
            //g.AddNode("4");
            //g.AddNode("5");
            //g.AddNode("6");
            //g.AddNode("7");
            //g.AddNode("8");
            //g.AddNode("9");
            //g.AddNode("10");
            //g.AddNode("11");
            //g.AddNode("12");
            //g.AddNode("13");
            //g.AddNode("S1");
            //g.AddNode("S2");
            //g.AddNode("S3");
            //g.AddNode("S4");
            //g.AddNode("S5");

            //g.AddEdge("1", "S3", 2);
            //g.AddEdge("1", "2");
            //g.AddEdge("1", "3");
            //g.AddEdge("1", "8");

            //g.AddEdge("2", "S2", 1);
            //g.AddEdge("2", "1");
            //g.AddEdge("2", "12");

            //g.AddEdge("3", "1");
            //g.AddEdge("3", "4");
            //g.AddEdge("3", "5");

            //g.AddEdge("4", "3");
            //g.AddEdge("4", "S4", 3);

            //g.AddEdge("5", "7");
            //g.AddEdge("5", "3");
            //g.AddEdge("5", "6");
            //g.AddEdge("5", "9");

            //g.AddEdge("6", "S1", 0);
            //g.AddEdge("6", "5");
            //g.AddEdge("6", "11");

            //g.AddEdge("7", "5");

            //g.AddEdge("8", "1");

            //g.AddEdge("9", "5");
            //g.AddEdge("9", "11");
            //g.AddEdge("9", "10");

            //g.AddEdge("10", "9");
            //g.AddEdge("10", "S5", 4);

            //g.AddEdge("11", "6");
            //g.AddEdge("11", "9");

            //g.AddEdge("12", "2");
            //g.AddEdge("12", "13");

            //g.AddEdge("13", "12");

            //State stateBegin = new State(AgentCount);
            //stateBegin.Agent[0] = g["2"];
            //stateBegin.Agent[1] = g["6"];
            //stateBegin.Agent[2] = g["7"];
            //stateBegin.Agent[3] = g["8"];
            //stateBegin.Agent[4] = g["13"];

            //State stateEnd = new State(AgentCount);
            //stateEnd.Agent[0] = g["S1"];
            //stateEnd.Agent[1] = g["S2"];
            //stateEnd.Agent[2] = g["S3"];
            //stateEnd.Agent[3] = g["S4"];
            //stateEnd.Agent[4] = g["S5"];
            //// -----------------------

            // 5 philizophers
            Graph g = new Graph();
            g.AddNode("B1");
            g.AddNode("B2");
            g.AddNode("B3");
            g.AddNode("B4");
            g.AddNode("B5");

            g.AddNode("_____");
            g.AddNode("A____");
            g.AddNode("_B___");
            g.AddNode("__C__");
            g.AddNode("___D_");
            g.AddNode("____E");
            g.AddNode("AB___");
            g.AddNode("A_C__");
            g.AddNode("A__D_");
            g.AddNode("A___E");
            g.AddNode("_BC__");
            g.AddNode("_B_D_");
            g.AddNode("_B__E");
            g.AddNode("__CD_");
            g.AddNode("__C_E");
            g.AddNode("___DE");
            g.AddNode("ABC__");
            g.AddNode("AB_D_");
            g.AddNode("AB__E");
            g.AddNode("A_CD_");
            g.AddNode("A_C_E");
            g.AddNode("A__DE");
            g.AddNode("_BCD_");
            g.AddNode("_BC_E");
            g.AddNode("_B_DE");
            g.AddNode("__CDE");
            g.AddNode("ABCD_");
            g.AddNode("ABC_E");
            g.AddNode("AB_DE");
            g.AddNode("A_CDE");
            g.AddNode("_BCDE");
            g.AddNode("ABCDE");

            g.AddNode("E1");
            g.AddNode("E2");
            g.AddNode("E3");
            g.AddNode("E4");
            g.AddNode("E5");

            var agentsA = new int[] { A(1), A(2) };
            var agentsB = new int[] { A(2), A(3) };
            var agentsC = new int[] { A(3), A(4) };
            var agentsD = new int[] { A(4), A(5) };
            var agentsE = new int[] { A(5), A(1) };

            g.AddEdge("_____", "A____", agentsA);
            g.AddEdge("_____", "_B___", agentsB);
            g.AddEdge("_____", "__C__", agentsC);
            g.AddEdge("_____", "___D_", agentsD);
            g.AddEdge("_____", "____E", agentsE);
            // -
            g.AddEdge("A____", "AB___", agentsB);
            g.AddEdge("A____", "A_C__", agentsC);
            g.AddEdge("A____", "A__D_", agentsD);
            g.AddEdge("A____", "A___E", agentsE);

            g.AddEdge("_B___", "AB___", agentsA);
            g.AddEdge("_B___", "_BC__", agentsC);
            g.AddEdge("_B___", "_B_D_", agentsD);
            g.AddEdge("_B___", "_B__E", agentsE);

            g.AddEdge("__C__", "A_C__", agentsA);
            g.AddEdge("__C__", "_BC__", agentsB);
            g.AddEdge("__C__", "__CD_", agentsD);
            g.AddEdge("__C__", "__C_E", agentsE);

            g.AddEdge("___D_", "A__D_", agentsA);
            g.AddEdge("___D_", "_B_D_", agentsB);
            g.AddEdge("___D_", "__CD_", agentsC);
            g.AddEdge("___D_", "___DE", agentsE);

            g.AddEdge("____E", "A___E", agentsA);
            g.AddEdge("____E", "_B__E", agentsB);
            g.AddEdge("____E", "__C_E", agentsC);
            g.AddEdge("____E", "___DE", agentsD);
            //// -
            //g.AddEdge("AB___", "ABC__", agentsC);
            //g.AddEdge("AB___", "AB_D_", agentsD);
            //g.AddEdge("AB___", "AB__E", agentsE);

            //g.AddEdge("A_C__", "ABC__", agentsB);
            //g.AddEdge("A_C__", "A_CD_", agentsD);
            //g.AddEdge("A_C__", "A_C_E", agentsE);

            //g.AddEdge("A__D_", "AB_D_", agentsB);
            //g.AddEdge("A__D_", "A_CD_", agentsC);
            //g.AddEdge("A__D_", "A__DE", agentsE);

            //g.AddEdge("A___E", "AB__E", agentsB);
            //g.AddEdge("A___E", "A_C_E", agentsC);
            //g.AddEdge("A___E", "A__DE", agentsD);

            //g.AddEdge("_BC__", "ABC__", agentsA);
            //g.AddEdge("_BC__", "_BCD_", agentsD);
            //g.AddEdge("_BC__", "_BC_E", agentsE);

            //g.AddEdge("_B_D_", "AB_D_", agentsA);
            //g.AddEdge("_B_D_", "_BCD_", agentsC);
            //g.AddEdge("_B_D_", "_B_DE", agentsE);

            //g.AddEdge("_B__E", "AB__E", agentsA);
            //g.AddEdge("_B__E", "_BC_E", agentsC);
            //g.AddEdge("_B__E", "_B_DE", agentsD);

            //g.AddEdge("__CD_", "A_CD_", agentsA);
            //g.AddEdge("__CD_", "_BCD_", agentsB);
            //g.AddEdge("__CD_", "__CDE", agentsE);

            //g.AddEdge("__C_E", "A_C_E", agentsA);
            //g.AddEdge("__C_E", "_BC_E", agentsB);
            //g.AddEdge("__C_E", "__CDE", agentsD);

            //g.AddEdge("___DE", "A__DE", agentsA);
            //g.AddEdge("___DE", "_B_DE", agentsB);
            //g.AddEdge("___DE", "__CDE", agentsC);
            //// -
            //g.AddEdge("ABC__", "ABCD_", agentsD);
            //g.AddEdge("ABC__", "ABC_E", agentsE);

            //g.AddEdge("AB_D_", "ABCD_", agentsC);
            //g.AddEdge("AB_D_", "AB_DE", agentsE);

            //g.AddEdge("AB__E", "ABC_E", agentsC);
            //g.AddEdge("AB__E", "AB_DE", agentsD);

            //g.AddEdge("A_CD_", "ABCD_", agentsB);
            //g.AddEdge("A_CD_", "A_CDE", agentsE);

            //g.AddEdge("A_C_E", "ABC_E", agentsB);
            //g.AddEdge("A_C_E", "A_CDE", agentsD);

            //g.AddEdge("A__DE", "AB_DE", agentsB);
            //g.AddEdge("A__DE", "A_CDE", agentsC);

            //g.AddEdge("_BCD_", "ABCD_", agentsA);
            //g.AddEdge("_BCD_", "_BCDE", agentsE);

            //g.AddEdge("_BC_E", "ABC_E", agentsA);
            //g.AddEdge("_BC_E", "_BCDE", agentsD);

            //g.AddEdge("_B_DE", "AB_DE", agentsA);
            //g.AddEdge("_B_DE", "_BCDE", agentsC);

            //g.AddEdge("__CDE", "A_CDE", agentsA);
            //g.AddEdge("__CDE", "_BCDE", agentsB);
            //// -
            //g.AddEdge("ABCD_", "ABCDE", agentsE);

            //g.AddEdge("ABC_E", "ABCDE", agentsD);

            //g.AddEdge("AB_DE", "ABCDE", agentsC);

            //g.AddEdge("A_CDE", "ABCDE", agentsB);

            //g.AddEdge("_BCDE", "ABCDE", agentsA);
            //// -

            // transition to start
            g.AddEdge("B1", "_____", A(1));
            g.AddEdge("B2", "_____", A(2));
            g.AddEdge("B3", "_____", A(3));
            g.AddEdge("B4", "_____", A(4));
            g.AddEdge("B5", "_____", A(5));

            // transition to end
            g.AddEdge("A___E", "E1", A(1));
            g.AddEdge("AB___", "E2", A(2));
            g.AddEdge("_BC__", "E3", A(3));
            g.AddEdge("__CD_", "E4", A(4));
            g.AddEdge("___DE", "E5", A(5));

            //g.AddEdge("ABC__", "E2", A(2));
            //g.AddEdge("AB_D_", "E2", A(2));
            //g.AddEdge("AB__E", "E2", A(2));
            //g.AddEdge("A_CD_", "E4", A(4));
            //g.AddEdge("A__DE", "E5", A(5));
            //g.AddEdge("_BCD_", "E3", A(3));
            //g.AddEdge("_BCD_", "E4", A(4));
            //g.AddEdge("_BC_E", "E3", A(3));
            //g.AddEdge("_B_DE", "E4", A(4));
            //g.AddEdge("__CDE", "E4", A(4));
            //g.AddEdge("__CDE", "E5", A(5));

            //g.AddEdge("ABCD_", "E2", A(2));
            //g.AddEdge("ABCD_", "E3", A(3));
            //g.AddEdge("ABCD_", "E4", A(4));
            //g.AddEdge("ABC_E", "E1", A(1));
            //g.AddEdge("ABC_E", "E2", A(2));
            //g.AddEdge("ABC_E", "E3", A(3));
            //g.AddEdge("AB_DE", "E1", A(1));
            //g.AddEdge("AB_DE", "E2", A(2));
            //g.AddEdge("AB_DE", "E5", A(5));
            //g.AddEdge("A_CDE", "E1", A(1));
            //g.AddEdge("A_CDE", "E4", A(4));
            //g.AddEdge("A_CDE", "E5", A(5));
            //g.AddEdge("_BCDE", "E3", A(3));
            //g.AddEdge("_BCDE", "E4", A(4));
            //g.AddEdge("_BCDE", "E5", A(5));

            //g.AddEdge("ABCDE", "E1");
            //g.AddEdge("ABCDE", "E2");
            //g.AddEdge("ABCDE", "E3");
            //g.AddEdge("ABCDE", "E4");
            //g.AddEdge("ABCDE", "E5");

            State stateBegin = new State(AgentCount);
            stateBegin.Agent[0] = g["B1"];
            stateBegin.Agent[1] = g["B2"];
            stateBegin.Agent[2] = g["B3"];
            stateBegin.Agent[3] = g["B4"];
            stateBegin.Agent[4] = g["B5"];

            State stateEnd = new State(AgentCount);
            stateEnd.Agent[0] = g["E1"];
            stateEnd.Agent[1] = g["E2"];
            stateEnd.Agent[2] = g["E3"];
            stateEnd.Agent[3] = g["E4"];
            stateEnd.Agent[4] = g["E5"];
            // -----------------------

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

            for (int i = 0; i < NewPopulationRandom; i++)
            {
                var state = GenerateRandomState(possibleNodes, AgentCount);
                var path = BuildPath(state, PathLen);
                population.Add(path);
                //Console.WriteLine(path.ToString());
            }
           
            for (int gen = 0; gen < GenCount; gen++)
            {
                Console.WriteLine($"Generation {gen}...");

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
                    var newMixes = Path.CombineOn(tmpPaths[0], tmpPaths[1], item.Key).ToList();

                    try
                    {
                        // verify mixes
                        for (int j = 0; j < newMixes.Count; j++)
                        {
                            var mix = newMixes[j];
                            for (int i = 0; i < mix.States.Count - 1; i++)
                            {
                                if (mix.States[i].SimilarityTo(mix.States[i + 1]) < ((double)(AgentCount - 1) / (double)AgentCount))
                                {
                                    throw new Exception("mix wrong!");
                                }
                            }
                        }
                    }
                    catch
                    {
                        var nm = Path.CombineOn(tmpPaths[0], tmpPaths[1], item.Key).ToList();

                    }

                    mixes.AddRange(newMixes);
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
                    .Take(TakePopulationFromPrev / 3)
                    .ToList();

                var similarToEnd = mixesAndPopulation
                    .OrderByDescending(x => x.States.Last().SimilarityTo(stateEnd))
                    .ThenByDescending(x => x.States.Count)
                    .Take(TakePopulationFromPrev / 3)
                    .ToList();

                var similarToBoth = mixesAndPopulation
                    .OrderByDescending(x => (x.States.First().SimilarityTo(stateBegin) + x.States.Last().SimilarityTo(stateEnd)) / 2)
                    .ThenBy(x => x.States.Count)
                    .Take(TakePopulationFromPrev / 3)
                    .ToList();

                population.Clear();
                population.AddRange(similarToStart);
                population.AddRange(similarToEnd);
                population.AddRange(similarToBoth);

                var haveOkStart = similarToStart.Where(x => x.States.First().SimilarityTo(stateBegin) == 1).Take(NewPopulationSimilar / 2).ToList();
                var haveOkEnd = similarToEnd.Where(x => x.States.Last().SimilarityTo(stateEnd) == 1).Take(NewPopulationSimilar / 2).ToList();

                foreach (var item in haveOkStart)
                {
                    population.Add(BuildPath(item.States.Last(), PathLen));

                }
                foreach (var item in haveOkEnd)
                {
                    population.Add(BuildReversePath(item.States.First(), PathLen));
                }

                for (int i = 0; i < NewPopulationRandom / 2; i++)
                {
                    var state = GenerateRandomState(possibleNodes, AgentCount);
                    population.Add(BuildPath(state, PathLen));
                    population.Add(BuildReversePath(state, PathLen));
                }

                var result = population.Find(x => x.States.First() == stateBegin && x.States.Last() == stateEnd);
                if (result != null)
                {
                    Console.WriteLine(result);
                    break;
                }
            }
        }


        static int A(int agent) => agent - 1;

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
