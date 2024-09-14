namespace FluentXmlWriterCore;

public interface IFluentXmlWriterSimple
	: IFluentXmlWriter
{
	IFluentXmlWriterSimple Attr(string name, string value);
	IFluentXmlWriterSimple Simple(string name);
	IFluentXmlWriterSimple ManySimple(params SimpleElement[] simpleElements);
	IFluentXmlWriterComplex Complex(string name);
	IFluentXmlWriterComplex Text(string text);
	IFluentXmlWriterComplex CData(string cdataText);
	IFluentXmlWriterComplex Comment(string comment);
	IFluentXmlWriterComplex EndElem();
	void OutputToString(Action<string> action);
	void OutputToFile(string fileName);
}
// TODO: Conditionals