using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.StateMachine
{
    public class StateMachineSystem
    {
        public List<Graph> Graphs { get; set; } = new List<Graph>();

        public Logic.System SystemReference { get; set; }

        public string ToDedan()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"#DEFINE N {SystemReference.Processes.Count}");
            sb.AppendLine();
            foreach (var graph in Graphs)
            {
                sb.AppendLine(graph.ToDedan(SystemReference));
            }

            var agents = SystemReference.Processes.Select(x => $"Agent{x.Name}");
            sb.AppendLine($"agents {string.Join(", ", agents)};");
            sb.AppendLine($"servers {string.Join(", ", SystemReference.Servers.Select(x => x.Name).Concat(SystemReference.Processes.Select(x => x.Name)))};");
            sb.AppendLine();
            sb.AppendLine("init -> {");
            foreach (var graph in Graphs)
            {
                var arguments = agents.Concat(SystemReference.GetAllDedanServerListExcept(graph.Name));
                sb.AppendLine($"    {graph.Name}({string.Join(", ", arguments)}).{graph.InitNode},");
            }
            sb.AppendLine();
            foreach (var process in SystemReference.Processes)
            {
                sb.AppendLine($"    Agent{process.Name}.{process.Name}.START_FROM_INIT,");
            }
            sb.Append("}.");

            return sb.ToString();
        }
    }
}
