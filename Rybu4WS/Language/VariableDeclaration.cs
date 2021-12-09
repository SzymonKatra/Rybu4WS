﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Rybu4WS.Language
{
    public class VariableDeclaration
    {
        public string Name { get; set; }

        public VariableType Type { get; set; }

        public List<string> AvailableValues { get; set; } = new List<string>();

        public string InitialValue { get; set; }

        public int? ArraySize { get; set; }
    }
}
