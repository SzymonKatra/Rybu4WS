using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Rybu4WS.TrailDebugger.TrailSchema.Types
{
    public struct UppercaseBool : IXmlSerializable
    {
        public bool Value { get; set; }

        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(XmlReader reader)
        {
            var content = reader.ReadElementContentAsString();
            Value = bool.Parse(content);
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteString(Value.ToString());
        }

        public static implicit operator bool(UppercaseBool x)
        {
            return x.Value;
        }

        public static implicit operator UppercaseBool(bool x)
        {
            return new UppercaseBool() { Value = x };
        }
    }
}
