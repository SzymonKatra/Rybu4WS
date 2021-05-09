using ServCompEvol.Algorithm;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServCompEvol
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var algorithm = FiveAgents();

            for (int i = 0; i < 100; i++)
            {
                var result = algorithm.FindGoal();
                if (result != null)
                {
                    Console.WriteLine(result.ToString());
                    break;
                }

                Console.WriteLine($"Generation {i}...");
                algorithm.Iterate();
            }
        }

        private static GeneticAlgorithm ThreeAgents()
        {
            var agent1 = new Agent("A1", 0);
            var agent2 = new Agent("A2", 1);
            var agent3 = new Agent("A3", 2);
            var agents = new List<Agent>() { agent1, agent2, agent3 };

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

            g.AddEdge("1", "S3", agent3);
            g.AddEdge("1", "2");
            g.AddEdge("1", "3");

            g.AddEdge("2", "S2", agent2);
            g.AddEdge("2", "1");

            g.AddEdge("3", "1");
            g.AddEdge("3", "4");
            g.AddEdge("3", "5");

            g.AddEdge("4", "3");

            g.AddEdge("5", "7");
            g.AddEdge("5", "3");
            g.AddEdge("5", "6");

            g.AddEdge("6", "S1", agent1);
            g.AddEdge("6", "5");

            g.AddEdge("7", "5");

            var algorithm = new GeneticAlgorithm(g, agents);
            algorithm.SetStart(g["2"], g["6"], g["7"]);
            algorithm.SetEnd(g["S1"], g["S2"], g["S3"]);

            return algorithm;
        }

        private static GeneticAlgorithm FourAgents()
        {
            var agent1 = new Agent("A1", 0);
            var agent2 = new Agent("A2", 1);
            var agent3 = new Agent("A3", 2);
            var agent4 = new Agent("A4", 3);
            var agents = new List<Agent>() { agent1, agent2, agent3, agent4 };

            Graph g = new Graph();
            g.AddNode("1");
            g.AddNode("2");
            g.AddNode("3");
            g.AddNode("4");
            g.AddNode("5");
            g.AddNode("6");
            g.AddNode("7");
            g.AddNode("8");
            g.AddNode("S1");
            g.AddNode("S2");
            g.AddNode("S3");
            g.AddNode("S4");

            g.AddEdge("1", "S3", agent3);
            g.AddEdge("1", "2");
            g.AddEdge("1", "3");
            g.AddEdge("1", "8");

            g.AddEdge("2", "S2", agent2);
            g.AddEdge("2", "1");

            g.AddEdge("3", "1");
            g.AddEdge("3", "4");
            g.AddEdge("3", "5");

            g.AddEdge("4", "3");
            g.AddEdge("4", "S4", agent4);

            g.AddEdge("5", "7");
            g.AddEdge("5", "3");
            g.AddEdge("5", "6");

            g.AddEdge("6", "S1", agent1);
            g.AddEdge("6", "5");

            g.AddEdge("7", "5");

            g.AddEdge("8", "1");

            var algorithm = new GeneticAlgorithm(g, agents);
            algorithm.SetStart(g["2"], g["6"], g["7"], g["8"]);
            algorithm.SetEnd(g["S1"], g["S2"], g["S3"], g["S4"]);

            return algorithm;
        }

        private static GeneticAlgorithm FiveAgents()
        {
            var agent1 = new Agent("A1", 0);
            var agent2 = new Agent("A2", 1);
            var agent3 = new Agent("A3", 2);
            var agent4 = new Agent("A4", 3);
            var agent5 = new Agent("A5", 4);
            var agents = new List<Agent>() { agent1, agent2, agent3, agent4, agent5 };

            Graph g = new Graph();
            g.AddNode("1");
            g.AddNode("2");
            g.AddNode("3");
            g.AddNode("4");
            g.AddNode("5");
            g.AddNode("6");
            g.AddNode("7");
            g.AddNode("8");
            g.AddNode("9");
            g.AddNode("10");
            g.AddNode("11");
            g.AddNode("12");
            g.AddNode("13");
            g.AddNode("S1");
            g.AddNode("S2");
            g.AddNode("S3");
            g.AddNode("S4");
            g.AddNode("S5");

            g.AddEdge("1", "S3", agent3);
            g.AddEdge("1", "2");
            g.AddEdge("1", "3");
            g.AddEdge("1", "8");

            g.AddEdge("2", "S2", agent2);
            g.AddEdge("2", "1");
            g.AddEdge("2", "12");

            g.AddEdge("3", "1");
            g.AddEdge("3", "4");
            g.AddEdge("3", "5");

            g.AddEdge("4", "3");
            g.AddEdge("4", "S4", agent4);

            g.AddEdge("5", "7");
            g.AddEdge("5", "3");
            g.AddEdge("5", "6");
            g.AddEdge("5", "9");

            g.AddEdge("6", "S1", agent1);
            g.AddEdge("6", "5");
            g.AddEdge("6", "11");

            g.AddEdge("7", "5");

            g.AddEdge("8", "1");

            g.AddEdge("9", "5");
            g.AddEdge("9", "11");
            g.AddEdge("9", "10");

            g.AddEdge("10", "9");
            g.AddEdge("10", "S5", agent5);

            g.AddEdge("11", "6");
            g.AddEdge("11", "9");

            g.AddEdge("12", "2");
            g.AddEdge("12", "13");

            g.AddEdge("13", "12");

            var algorithm = new GeneticAlgorithm(g, agents);
            algorithm.SetStart(g["2"], g["6"], g["7"], g["8"], g["13"]);
            algorithm.SetEnd(g["S1"], g["S2"], g["S3"], g["S4"], g["S4"]);

            return algorithm;
        }
    }
}
