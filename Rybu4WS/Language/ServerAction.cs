﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.Language
{
    public class ServerAction
    {
        public string Name { get; set; }

        public List<ServerActionBranch> Branches { get; set; } = new List<ServerActionBranch>();

        public List<string> Callers { get; set; } = new List<string>();

        public List<string> PossibleReturnValues { get; set; } = new List<string>();

        public bool CanTerminate { get; set; } = false;
    }
}
