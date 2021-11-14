using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Rybu4WS.TrailDebugger.TrailSchema
{
    public class AgentVar
    {
        [XmlElement("agent_tag")]
        public int AgentTag { get; set; }
        
        [XmlElement("agent_var_name")]
        public string AgentVarName { get; set; }

        [XmlElement("agent_ini_server")]
        public string AgentIniServer { get; set; }

        [XmlElement("agent_ini_service")]
        public string AgentIniService { get; set; }
    }
}
