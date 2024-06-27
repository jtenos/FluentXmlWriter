using System.Xml;
using System.IO;
namespace FluentXmlWriter
{
    public class CustomXmlWriter
        : XmlTextWriter
    {
        public CustomXmlWriter(StringWriter sw)
            : base(sw)
        {
            Indentation = 4;
            IndentChar = ' ';
            Formatting = Formatting.Indented;
        }

        public override void WriteStartDocument()
        {
        }
        public override void WriteStartDocument(bool standalone)
        {
        }
    }
}
