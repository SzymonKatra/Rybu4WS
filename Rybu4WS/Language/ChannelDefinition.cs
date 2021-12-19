using System;
using System.Collections.Generic;
using System.Text;

namespace Rybu4WS.Language
{
    public class ChannelDefinition : IWithCodeLocation
    {
        public string SourceServer { get; set; }

        public string TargetServer { get; set; }

        public TimedDelay Delay { get; set; }

        public CodeLocation CodeLocation { get; set; }
    }
}
