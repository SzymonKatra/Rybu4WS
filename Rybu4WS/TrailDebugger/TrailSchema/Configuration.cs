using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Rybu4WS.TrailDebugger.TrailSchema
{
    public class Configuration
    {
        [XmlElement("conf_nr")]
        public int ConfNr { get; set; }

        [XmlArray("states")]
        [XmlArrayItem("state")]
        public State[] States { get; set; }

        [XmlArray("messages")]
        [XmlArrayItem("message")]
        public Message[] Messages { get; set; }

        [XmlArray("actions")]
        [XmlArrayItem("action")]
        public Action[] Actions { get; set; }
    }
}
