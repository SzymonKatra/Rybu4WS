﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.Language
{
    public class System
    {
        public List<ServerDeclaration> ServerDeclarations { get; set; } = new List<ServerDeclaration>();

        public List<ServerDefinition> ServerDefinitions { get; set; } = new List<ServerDefinition>();

        public List<Server> Servers { get; set; } = new List<Server>();

        public List<Process> Processes { get; set; } = new List<Process>();

        public IEnumerable<string> GetAllDedanServerListExcept(string dedanServerName)
        {
            return Servers.Select(x => x.Name).Concat(Processes.Select(x => x.ServerName)).Except(new[] { dedanServerName }).OrderBy(x => x);
        }
    }
}
