using Rybu4WS.TrailDebugger.TrailSchema.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Rybu4WS.TrailDebugger.TrailSchema
{
    public class Message
    {
        [XmlElement("agent_tag")]
        public int AgentTag { get; set; }

        [XmlElement("agent")]
        public string Agent { get; set; }

        [XmlElement("server_tag")]
        public int ServerTag { get; set; }

        [XmlElement("server")]
        public string Server { get; set; }

        [XmlElement("service")]
        public string Service { get; set; }

        [XmlElement("active")]
        public UppercaseBool Active { get; set; }
    }
}
