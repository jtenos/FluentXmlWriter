using System.Text;
using System.Xml;

namespace FluentXmlWriterCore;

public partial class FluentXmlWriter
	: IDisposable, IFluentXmlWriterComplex, IFluentXmlWriterSimple
{
	private readonly StringBuilder? _stringBuilder;
	private readonly TextWriter _textWriter;
	private readonly XmlWriter _xmlWriter;
	private readonly bool _isStreamMode;

	private FluentXmlWriter(FormattingOptions? options)
	{
		_stringBuilder = new StringBuilder();
		_textWriter = new StringWriter(_stringBuilder);
		_xmlWriter = new CustomXmlWriter(_textWriter, options);
		_isStreamMode = false;
	}

	private FluentXmlWriter(FluentXmlWriter writer)
	{
		_stringBuilder = writer._stringBuilder;
		_textWriter = writer._textWriter;
		_xmlWriter = writer._xmlWriter;
		_isStreamMode = writer._isStreamMode;
	}

	private FluentXmlWriter(TextWriter textWriter, FormattingOptions? options, bool isStreamMode)
	{
		_stringBuilder = null;
		_textWriter = textWriter;
		_xmlWriter = new CustomXmlWriter(_textWriter, options);
		_isStreamMode = isStreamMode;
	}

	public static IFluentXmlWriterComplex Start(string topLevelElement)
	{
		IFluentXmlWriterComplex fluentXmlWriter = new FluentXmlWriter((FormattingOptions?)null);
		return fluentXmlWriter.Complex(topLevelElement);
	}

	public static IFluentXmlWriterComplex Start(string topLevelElement, FormattingOptions options)
	{
		IFluentXmlWriterComplex fluentXmlWriter = new FluentXmlWriter(options);
		return fluentXmlWriter.Complex(topLevelElement);
	}

	public static IFluentXmlWriterComplex Start(string topLevelElement, bool indented)
	{
		var options = indented 
			? FormattingOptions.Default.WithTabs().WithNewLine(Environment.NewLine)
			: FormattingOptions.Default;
		IFluentXmlWriterComplex fluentXmlWriter = new FluentXmlWriter(options);
		return fluentXmlWriter.Complex(topLevelElement);
	}

	public static IFluentXmlWriterComplex Start(TextWriter textWriter, FormattingOptions? options = null)
	{
		IFluentXmlWriterComplex fluentXmlWriter = new FluentXmlWriter(textWriter, options, true);
		return fluentXmlWriter;
	}

	public static IFluentXmlWriterComplex Start(Stream stream, FormattingOptions? options = null, Encoding? encoding = null)
	{
		var streamWriter = new StreamWriter(stream, encoding ?? Encoding.UTF8);
		IFluentXmlWriterComplex fluentXmlWriter = new FluentXmlWriter(streamWriter, options, true);
		return fluentXmlWriter;
	}

	public override string ToString() => _stringBuilder?.ToString() ?? string.Empty;

	void IDisposable.Dispose() => ((IDisposable)_xmlWriter).Dispose();

	private static string ReformatXml(string xml, FormattingOptions options)
	{
		var doc = new System.Xml.XmlDocument();
		doc.LoadXml(xml);
		
		var sb = new StringBuilder();
		
		// Ensure we have valid indent character when indentation is needed
		var indentChar = options.IndentChar;
		if (indentChar == '\0' && options.Indentation > 0)
		{
			indentChar = '\t'; // Default to tab if null character with indentation
		}
		
		var settings = new System.Xml.XmlWriterSettings
		{
			Indent = options.Indent,
			IndentChars = new string(indentChar, options.Indentation),
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
