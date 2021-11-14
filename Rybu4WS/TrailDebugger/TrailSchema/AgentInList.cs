using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Rybu4WS.TrailDebugger.TrailSchema
{
    public class AgentInList
    {
        [XmlElement("agent_list_tag")]
        public int AgentListTag { get; set; }

        [XmlElement("agent_list_name")]
        public string AgentListName { get; set; }
    }
}
