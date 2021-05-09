using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServCompEvol.Algorithm
{
    public class GeneticAlgorithm
    {
        private Graph _graph;
        private List<Agent> _agents;
        private State _startState;
        private State _endState;
        private Random _rnd;

        private List<Node> _nonTerminatingNodes;

        private List<Path> _population;
        public const int SelectionCount = 60;
        public const int RandomSupplyCount = 40;
        public const int SupplyStartPathsCount = 20;
        public const int SupplyEndPathsCount = 20;
        public const int PathGenerationLength = 20;

        public List<Path> Population => _population;

        public GeneticAlgorithm(Graph graph, List<Agent> agents)
        {
            _graph = graph;
            _agents = agents;
            _rnd = new Random();
            _population = new List<Path>();

            _nonTerminatingNodes = graph.Nodes.Where(n => !n.IsTerminationNode).ToList();
        }

        public void SetStart(params Node[] agentStates)
        {
            var state = new State(_agents);
            for (int i = 0; i < agentStates.Length; i++)
            {
                state.AgentStates[i] = agentStates[i];
            }
            SetStart(state);
        }

        public void SetStart(State state)
        {
            _startState = state;
        }

        public void SetEnd(params Node[] agentStates)
        {
            var state = new State(_agents);
            for (int i = 0; i < agentStates.Length; i++)
            {
                state.AgentStates[i] = agentStates[i];
            }
            SetEnd(state);
        }
        public void SetEnd(State state)
        {
            _endState = state;
        }

        public Path FindGoal()
        {
            return _population.Find(x => x.States.First() == _startState && x.States.Last() == _endState);
        }

        public void Iterate()
        {
            SupplyPopulation();

            // Selection
            _population.RemoveAll(x => x.States.Count <= 1);

            // Remove duplicated paths (the ones that start and end in the same state)
            var samePathIndex = _population.FindIndex(x => _population.Any(y => !object.ReferenceEquals(x, y) && x.HaveSameStartAndEnd(y)));
            while (samePathIndex != -1)
            {
                var samePath = _population[samePathIndex];
                var secSamePathIndex = _population.FindLastIndex(x => x.HaveSameStartAndEnd(samePath));
                _population.RemoveAt(secSamePathIndex);
                samePathIndex = _population.FindIndex(x => _population.Any(y => !object.ReferenceEquals(x, y) && x.HaveSameStartAndEnd(y)));
            }

            // Select paths that have states similar to initial state or goal
            var similarToStart = _population
                //.Where(x => x.SimiliarityToStart > 0)
                .OrderByDescending(x => x.SimiliarityToStart).ThenByDescending(x => x.States.Count)
                .Take(SelectionCount / 3)
                .ToList();

            var similarToEnd = _population.Except(similarToStart)
                //.Where(x => x.SimiliarityToEnd > 0)
                .OrderByDescending(x => x.SimiliarityToEnd).ThenByDescending(x => x.States.Count)
                .Take(SelectionCount / 3)
                .ToList();

            var similarToBoth = _population.Except(similarToStart).Except(similarToEnd)
                //.Where(x => x.SimiliarityAverage > 0)
                .OrderByDescending(x => x.SimiliarityAverage).ThenBy(x => x.States.Count)
                .Take(SelectionCount / 3)
                .ToList();

            _population.Clear();
            //_population.AddRange(similarToStart.Concat(similarToEnd).Concat(similarToBoth));

            // Reproduction
            var groupedSimilarStart = GroupFilterByStates(similarToStart);
            var groupedSimilarEnd = GroupFilterByStates(similarToEnd);
            var groupedSimilarBoth = GroupFilterByStates(similarToStart);

            var toCross = new List<(Path a, Path b, State state)>();
            foreach (var item in groupedSimilarStart)
            {
                if (groupedSimilarEnd.TryGetValue(item.Key, out var endCollection))
                {
                    var a = item.Value.OrderByDescending(x => x.SimiliarityToStart).OrderByDescending(x => x.States.Count).First();
                    var b = endCollection.OrderByDescending(x => x.SimiliarityToEnd).OrderByDescending(x => x.States.Count).First();
                    toCross.Add((a, b, item.Key));
                }

                if (groupedSimilarBoth.TryGetValue(item.Key, out var bothCollection))
                {
                    var a = item.Value.OrderByDescending(x => x.SimiliarityToStart).OrderByDescending(x => x.States.Count).First();
                    var b = bothCollection.OrderByDescending(x => x.SimiliarityAverage).OrderByDescending(x => x.States.Count).First();
                    toCross.Add((a, b, item.Key));
                }
            }

            foreach (var item in groupedSimilarEnd)
            {
                if (groupedSimilarBoth.TryGetValue(item.Key, out var bothCollection))
                {
                    var a = item.Value.OrderByDescending(x => x.SimiliarityToEnd).OrderByDescending(x => x.States.Count).First();
                    var b = bothCollection.OrderByDescending(x => x.SimiliarityAverage).OrderByDescending(x => x.States.Count).First();
                    toCross.Add((a, b, item.Key));
                }
            }

            // Crossover
            foreach (var item in toCross)
            {
                var first = CrossoverPath(item.a, item.b, item.state);
                first.ComputeSimiliarities(_startState, _endState);
                _population.Add(first);

                var second = CrossoverPath(item.b, item.a, item.state);
                second.ComputeSimiliarities(_startState, _endState);
                _population.Add(second);
            }
        }

        private Path CrossoverPath(Path a, Path b, State s)
        {
            var aIndex = a.States.IndexOf(s);
            var bIndex = b.States.IndexOf(s);

            var states = new List<State>();
            for (int i = 0; i <= aIndex; i++)
            {
                states.Add(a.States[i].Clone());
            }
            for (int i = bIndex + 1; i < b.States.Count; i++)
            {
                states.Add(b.States[i].Clone());
            }
            var result = new Path(states);
            result.RemoveLoops();
            return result;
        }

        private Dictionary<State, List<Path>> GroupFilterByStates(IEnumerable<Path> individuals)
        {
            var grouped = new Dictionary<State, List<Path>>();

            foreach (var path in individuals)
            {
                foreach (var item in path.States)
                {
                    if (grouped.ContainsKey(item))
                    {
                        grouped[item].Add(path);
                    }
                    else
                    {
                        grouped.Add(item, new List<Path> { path });
                    }
                }
            }

            grouped = grouped
                .Where(x => x.Key != _startState && x.Key != _endState)
                .ToDictionary(x => x.Key, x => x.Value);

            return grouped;
        }

        public void SupplyPopulation()
        {
            var haveCorrectStart = _population.Where(x => x.SimiliarityToStart == 1).Take(SupplyStartPathsCount / 2).ToList();
            var haveCorrectEnd = _population.Where(x => x.SimiliarityToEnd == 1).Take(SupplyEndPathsCount / 2).ToList();

            for (int i = 0; i < SupplyStartPathsCount; i++)
            {
                var state = i < haveCorrectStart.Count ? haveCorrectStart[i].States.Last() : _startState;

                var path = GenerateRandomPath(state, PathGenerationLength);
                path.ComputeSimiliarities(_startState, _endState);
                _population.Add(path);
            }

            for (int i = 0; i < SupplyEndPathsCount; i++)
            {
                var state = i < haveCorrectEnd.Count ? haveCorrectEnd[i].States.First() : _endState;

                var path = GenerateRandomReversePath(state, PathGenerationLength);
                path.ComputeSimiliarities(_startState, _endState);
                _population.Add(path);
            }

            for (int i = 0; i < RandomSupplyCount / 2; i++)
            {
                var state = GenerateRandomState();

                var path = GenerateRandomPath(state, PathGenerationLength);
                path.ComputeSimiliarities(_startState, _endState);
                _population.Add(path);

                var revPath = GenerateRandomReversePath(state, PathGenerationLength);
                revPath.ComputeSimiliarities(_startState, _endState);
                _population.Add(revPath);
            }
        }

        private State GenerateRandomState()
        {
            var state = new State(_agents);

            for (int i = 0; i < state.AgentStates.Length; i++)
            {
                int nodeIndex = _rnd.Next(0, _nonTerminatingNodes.Count);
                int originalNodeIndex = nodeIndex;
                var node = _nonTerminatingNodes[nodeIndex];
                while (state.Contains(node))
                {
                    nodeIndex = (nodeIndex + 1) % _nonTerminatingNodes.Count;
                    node = _nonTerminatingNodes[nodeIndex];
                    if (nodeIndex == originalNodeIndex) throw new Exception("Cannot generate unique random state");
                }
                state.AgentStates[i] = node;
            }

            return state;
        }

        private Path GenerateRandomPath(State start, int depth)
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

                    var agentIndex = _rnd.Next(0, next.AgentStates.Length);
                    var originalAgentIndex = agentIndex;
                    while (next.AgentStates[agentIndex].OutEdges.Count == 0)
                    {
                        agentIndex = (agentIndex + 1) % next.AgentStates.Length;
                        if (agentIndex == originalAgentIndex) return states;
                    }

                    var outEdgeIndex = _rnd.Next(0, next.AgentStates[agentIndex].OutEdges.Count);
                    var originalOutEdgeIndex = outEdgeIndex;
                    var edge = next.AgentStates[agentIndex].OutEdges[outEdgeIndex];
                    while (next.Contains(edge.Target) || !edge.CanAgentUse(next.GetAgentAt(agentIndex)))
                    {
                        outEdgeIndex = (outEdgeIndex + 1) % next.AgentStates[agentIndex].OutEdges.Count;
                        if (outEdgeIndex == originalOutEdgeIndex) return states;
                        edge = next.AgentStates[agentIndex].OutEdges[outEdgeIndex];
                    }

                    next.AgentStates[agentIndex] = edge.Target;

                    if (states.Contains(next)) return states; // detected loop

                    states.Add(next);
                    current = next;
                }

                return states;
            }

            return new Path(generate().ToList());
        }

        private Path GenerateRandomReversePath(State end, int depth)
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

                    var agentIndex = _rnd.Next(0, next.AgentStates.Length);
                    var originalAgentIndex = agentIndex;
                    while (next.AgentStates[agentIndex].InEdges.Count == 0)
                    {
                        agentIndex = (agentIndex + 1) % next.AgentStates.Length;
                        if (agentIndex == originalAgentIndex) return states;
                    }

                    var inEdgeIndex = _rnd.Next(0, next.AgentStates[agentIndex].InEdges.Count);
                    var originalInEdgeIndex = inEdgeIndex;
                    var edge = next.AgentStates[agentIndex].InEdges[inEdgeIndex];
                    while (next.Contains(edge.Source) || !edge.CanAgentUse(next.GetAgentAt(agentIndex)))
                    {
                        inEdgeIndex = (inEdgeIndex + 1) % next.AgentStates[agentIndex].InEdges.Count;
                        if (inEdgeIndex == originalInEdgeIndex) return states;
                        edge = next.AgentStates[agentIndex].InEdges[inEdgeIndex];
                    }

                    next.AgentStates[agentIndex] = edge.Source;

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
