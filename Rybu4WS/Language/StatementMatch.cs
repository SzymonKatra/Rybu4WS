﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.Language
{
    public class StatementMatch : BaseStatement
    {
        public string ServerName { get; set; }

        public string ActionName { get; set; }

        public ServerAction ServerActionReference { get; set; }

        public List<StatementMatchOption> Handlers { get; set; } = new List<StatementMatchOption>();
    }
}
