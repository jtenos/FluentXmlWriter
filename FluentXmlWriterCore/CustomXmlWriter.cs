using System.Xml;

namespace FluentXmlWriterCore;

public class CustomXmlWriter
	: XmlTextWriter
{
	public CustomXmlWriter(StringWriter sw)
		: base(sw)
	{
		Indentation = 1;
		IndentChar = '\t';
		Formatting = Formatting.Indented;
	}

	public override void WriteStartDocument() { }
	public override void WriteStartDocument(bool standalone) { }
}
