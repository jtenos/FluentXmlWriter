using System.Text;
using System.Xml;

namespace FluentXmlWriterCore;

public partial class FluentXmlWriter
	: IDisposable, IFluentXmlWriterComplex, IFluentXmlWriterSimple
{
	private readonly StringBuilder _stringBuilder;
	private readonly StringWriter _stringWriter;
	private readonly XmlWriter _xmlWriter;

	private FluentXmlWriter(FluentXmlWriter? writer, FormattingOptions? options = null)
	{
		if (writer is null)
		{
			_stringBuilder = new StringBuilder();
			_stringWriter = new StringWriter(_stringBuilder);
			_xmlWriter = new CustomXmlWriter(_stringWriter, options);
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

	public static IFluentXmlWriterComplex Start(string topLevelElement, FormattingOptions options)
	{
		IFluentXmlWriterComplex fluentXmlWriter = new FluentXmlWriter(null, options);
		return fluentXmlWriter.Complex(topLevelElement);
	}

	public static IFluentXmlWriterComplex Start(string topLevelElement, bool indented)
	{
		var options = indented 
			? FormattingOptions.Default.WithTabs().WithNewLine(Environment.NewLine)
			: FormattingOptions.Default;
		IFluentXmlWriterComplex fluentXmlWriter = new FluentXmlWriter(null, options);
		return fluentXmlWriter.Complex(topLevelElement);
	}

	public override string ToString() => _stringBuilder.ToString();

	void IDisposable.Dispose() => ((IDisposable)_xmlWriter).Dispose();

	private static string ReformatXml(string xml, FormattingOptions options)
	{
		var doc = new System.Xml.XmlDocument();
		doc.LoadXml(xml);
		
		var sb = new StringBuilder();
		var settings = new System.Xml.XmlWriterSettings
		{
			Indent = options.Indent,
			IndentChars = new string(options.IndentChar, options.Indentation),
			NewLineChars = options.NewLine,
			NewLineOnAttributes = options.NewLineOnAttributes,
			OmitXmlDeclaration = true
		};
		
		using (var writer = System.Xml.XmlWriter.Create(sb, settings))
		{
			doc.Save(writer);
		}
		
		return sb.ToString();
	}
}
