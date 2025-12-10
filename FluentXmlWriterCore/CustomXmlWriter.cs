using System.Xml;

namespace FluentXmlWriterCore;

public class CustomXmlWriter
	: XmlTextWriter
{
	public CustomXmlWriter(StringWriter sw, FormattingOptions? options = null)
		: base(sw)
	{
		var opts = options ?? FormattingOptions.Default;
		
		if (opts.Indent)
		{
			Formatting = Formatting.Indented;
			Indentation = opts.Indentation;
			IndentChar = opts.IndentChar;
		}
		else
		{
			Formatting = Formatting.None;
		}
	}

	public override void WriteStartDocument() { }
	public override void WriteStartDocument(bool standalone) { }
}
