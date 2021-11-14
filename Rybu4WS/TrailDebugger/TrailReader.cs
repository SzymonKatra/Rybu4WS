using Rybu4WS.TrailDebugger.TrailSchema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Rybu4WS.TrailDebugger
{
    public class TrailReader
    {
        private XmlSerializer _serializer;

        public TrailReader()
        {
            _serializer = new XmlSerializer(typeof(Trail), new XmlRootAttribute("trail"));
        }

        public Trail Parse(string input)
        {
            return (Trail)_serializer.Deserialize(new StringReader(input));
        }
    }
}
