using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Rybu4WS.TrailDebugger.TrailSchema
{
    public class Action
    {
        [XmlElement("action_tag")]
        public int ActionTag { get; set; }

        [XmlElement("agent_tag")]
        public int AgentTag { get; set; }

        [XmlIgnore]
        public AgentVar AgentReference { get; set; }

        [XmlElement("server_tag")]
        public int ServerTag { get; set; }

        [XmlIgnore]
        public ServerVar ServerReference { get; set; }

        [XmlElement("state")]
        public string State { get; set; }

        [XmlElement("service")]
        public string Service { get; set; }

        [XmlElement("next_server_tag")]
        public int NextServerTag { get; set; }

        [XmlIgnore]
        public ServerVar NextServerReference { get; set; }
    }
}
