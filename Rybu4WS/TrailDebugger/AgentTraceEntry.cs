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
            MissingCode,
            Calling,
            Returned
        }

        public string ServerName { get; set; }

        public CodeLocation? CodeLocation { get; set; }

        public EntryState State { get; set; } = EntryState.None;

        public string CallingActionName { get; set; }

        public string ReturnValue { get; set; }
    }
}
