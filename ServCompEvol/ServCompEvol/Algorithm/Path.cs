using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServCompEvol.Algorithm
{
    public class Path
    {
        public List<State> States { get; }

        public State Start => States.First();
        public State End => States.Last();

        public double? SimiliarityToStart { get; private set; }
        public double? SimiliarityToEnd { get; private set; }
        public double? SimiliarityAverage { get; private set; }

        public Path(List<State> states)
        {
            States = states;
        }

        public void ComputeSimiliarities(State stateStart, State stateEnd)
        {
            SimiliarityToStart = States.First().SimilarityTo(stateStart);
            SimiliarityToEnd = States.Last().SimilarityTo(stateEnd);
            SimiliarityAverage = (SimiliarityToStart + SimiliarityToEnd) / 2.0;
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

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var item in States)
            {
                sb.AppendLine(item.ToString());
            }
            return sb.ToString();
        }
    }
}
