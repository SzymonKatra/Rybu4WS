using Rybu4WS.StateMachine.Composed;
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

        public List<ComposedGraph> ComposedGraphs { get; set; } = new List<ComposedGraph>();

        public Language.System SystemReference { get; set; }

        public string ToDedan()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"#DEFINE N {SystemReference.AgentCount}");
            sb.AppendLine();
            foreach (var graph in Graphs)
            {
                sb.AppendLine(graph.ToDedan(SystemReference));
            }
            foreach (var composedGraph in ComposedGraphs)
            {
                sb.AppendLine(composedGraph.ToDedan(SystemReference));
            }

            var agents = SystemReference.Processes.Select(x => x.Name)
                .Concat(SystemReference.Groups.SelectMany(x => x.Processes).Select(x => x.Name));
            sb.AppendLine($"agents {string.Join(", ", agents)};");

            var servers = SystemReference.Servers.Select(x => x.Name)
                .Concat(SystemReference.Processes.Select(x => x.ServerName))
                .Concat(SystemReference.Groups.Select(x => x.ServerName));
            sb.AppendLine($"servers {string.Join(", ", servers)};");

            if (SystemReference.TimedChannels.Any())
            {
                sb.AppendLine();
                sb.AppendLine("channels {");
                for (int i = 0; i < SystemReference.TimedChannels.Count; i++)
                {
                    var channel = SystemReference.TimedChannels[i];
                    if (channel.SourceServer != null && channel.TargetServer != null)
                    {
                        sb.Append($"    {channel.SourceServer} -> {channel.TargetServer} {channel.Delay.ToDedan()}");
                    }
                    else
                    {
                        sb.Append($"    {channel.Delay.ToDedan()}");
                    }

                    sb.AppendLine(i == SystemReference.TimedChannels.Count - 1 ? "" : ",");
                }
                sb.AppendLine("};");
            }

            sb.AppendLine();
            sb.AppendLine("init -> {");
            foreach (var graph in Graphs)
            {
                var arguments = agents.Concat(SystemReference.GetAllDedanServerListExcept(graph.Name));
                sb.AppendLine($"    {graph.Name}({string.Join(", ", arguments)}).{graph.InitNode},");
            }
            foreach (var composedGraph in ComposedGraphs)
            {
                var arguments = agents.Concat(SystemReference.GetAllDedanServerListExcept(composedGraph.Name));
                sb.AppendLine($"    {composedGraph.Name}({string.Join(", ", arguments)}).{composedGraph.InitNode},");
            }
            sb.AppendLine();
            foreach (var process in SystemReference.Processes)
            {
                sb.AppendLine($"    {process.Name}.{process.ServerName}.START_FROM_INIT,");
            }
            foreach (var group in SystemReference.Groups)
            {
                foreach (var process in group.Processes)
                {
                    sb.AppendLine($"    {process.Name}.{group.ServerName}.START_FROM_INIT,");
                }
            }
            sb.Append("}.");

            return sb.ToString();
        }
    }
}
