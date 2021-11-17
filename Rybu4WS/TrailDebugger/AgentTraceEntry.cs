using Rybu4WS.Language;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rybu4WS.TrailDebugger
{
    public class AgentTraceEntry
    {
        public enum EntryState
        {
            None,
            Pre,
            At,
            MissingCode
        }

        public CodeLocation CodeLocation { get; set; }

        public EntryState State { get; set; } = EntryState.None;
    }
}
