using System.Text;
using System.Xml;

namespace FluentXmlWriterCore;

public partial class FluentXmlWriter
	: IDisposable, IFluentXmlWriterComplex, IFluentXmlWriterSimple
{
	private readonly StringBuilder _stringBuilder;
	private readonly StringWriter _stringWriter;
	private readonly XmlWriter _xmlWriter;

	private FluentXmlWriter(FluentXmlWriter? writer)
	{
		if (writer is null)
		{
			_stringBuilder = new StringBuilder();
			_stringWriter = new StringWriter(_stringBuilder);
			_xmlWriter = new CustomXmlWriter(_stringWriter);
		}
		else
		{
			_stringBuilder = writer._stringBuilder;
			_stringWriter = writer._stringWriter;
			_xmlWriter = writer._xmlWriter;
		}
	}

	public static IFluentXmlWriterComplex Start(string topLevelElement)
	{
		IFluentXmlWriterComplex fluentXmlWriter = new FluentXmlWriter(null);
		return fluentXmlWriter.Complex(topLevelElement);
	}

	public override string ToString() => _stringBuilder.ToString();

	void IDisposable.Dispose() => ((IDisposable)_xmlWriter).Dispose();
}
