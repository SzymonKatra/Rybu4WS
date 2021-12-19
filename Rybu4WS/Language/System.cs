using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.Language
{
    public class System
    {
        public List<ConstDeclaration> ConstDeclarations { get; set; } = new List<ConstDeclaration>();

        public List<VariableTypeDefinition> TypeDefinitions { get; set; } = new List<VariableTypeDefinition>();

        public List<InterfaceDeclaration> InterfaceDeclarations { get; set; } = new List<InterfaceDeclaration>();

        public List<ServerDeclaration> ServerDeclarations { get; set; } = new List<ServerDeclaration>();

        public List<ServerDefinition> ServerDefinitions { get; set; } = new List<ServerDefinition>();

        public List<Server> Servers { get; set; } = new List<Server>();

        public List<ChannelDefinition> TimedChannels { get; set; } = new List<ChannelDefinition>();

        public List<Process> Processes { get; set; } = new List<Process>();

        public List<Group> Groups { get; set; } = new List<Group>();

        public int AgentCount => Processes.Count + Groups.Sum(x => x.Processes.Count);

        public IEnumerable<string> GetAllDedanServerListExcept(string dedanServerName)
        {
            return Servers.Select(x => x.Name)
                .Concat(Processes.Select(x => x.ServerName))
                .Concat(Groups.Select(x => x.ServerName))
                .Except(new[] { dedanServerName })
                .OrderBy(x => x);
        }
    }
}
