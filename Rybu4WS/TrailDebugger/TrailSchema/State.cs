using Rybu4WS.TrailDebugger.TrailSchema.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Rybu4WS.TrailDebugger.TrailSchema
{
    public class State
    {
        [XmlElement("server_tag")]
        public int ServerTag { get; set; }

        [XmlElement("server")]
        public string Server { get; set; }

        [XmlElement("value")]
        public string Value { get; set; }

        [XmlElement("active")]
        public UppercaseBool Active { get; set; }
    }
}
