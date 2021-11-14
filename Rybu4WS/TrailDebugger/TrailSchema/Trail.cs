using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Rybu4WS.TrailDebugger.TrailSchema
{
    public class Trail
    {
        [XmlArray("server_vars")]
        [XmlArrayItem("server_var")]
        public ServerVar[] ServerVars { get; set; }

        [XmlArray("agent_vars")]
        [XmlArrayItem("agent_var")]
        public AgentVar[] AgentVars { get; set; }

        [XmlArray("agent_list")]
        [XmlArrayItem("agent_in_list")]
        public AgentInList[] AgentList { get; set; }

        [XmlArray("configurations")]
        [XmlArrayItem("configuration")]
        public Configuration[] Configurations { get; set; }
    }
}
