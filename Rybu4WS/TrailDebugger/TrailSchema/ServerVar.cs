using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Rybu4WS.TrailDebugger.TrailSchema
{
    public class ServerVar
    {
        [XmlElement("server_tag")]
        public int ServerTag { get; set; }

        [XmlElement("server_var_name")]
        public string ServerVarName { get; set; }

        [XmlElement("server_var_state")]
        public string ServerVarState { get; set; }
    }
}
