namespace FluentXmlWriterCore;

public interface IFluentXmlWriterComplex
   : IFluentXmlWriter
{
	IFluentXmlWriterComplex Attr(string name, string value);
	IFluentXmlWriterSimple Simple(string name);
	IFluentXmlWriterSimple ManySimple(params SimpleElement[] simpleElements);
	IFluentXmlWriterComplex Complex(string name);
	IFluentXmlWriterComplex Text(string text);
	IFluentXmlWriterComplex CData(string cdataText);
	IFluentXmlWriterComplex Comment(string comment);
	IFluentXmlWriterComplex EndElem();
	void OutputToString(Action<string> action);
	string OutputToString();
	string OutputToString(bool indented);
	string OutputToString(FormattingOptions options);
	void OutputToFile(string fileName);
	void OutputToFile(string fileName, bool indented);
	void OutputToFile(string fileName, FormattingOptions options);
}
// TODO: Conditionals (ex: SimpleIf(someCond, ...))