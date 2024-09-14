namespace FluentXmlWriterCore;

partial class FluentXmlWriter
{
	IFluentXmlWriterSimple IFluentXmlWriterSimple.Attr(string name, string value)
	{
		_xmlWriter.WriteAttributeString(name, value);
		return this;
	}

	IFluentXmlWriterComplex IFluentXmlWriterSimple.Complex(string name)
	{
		_xmlWriter.WriteEndElement();
		_xmlWriter.WriteStartElement(name);
		return this;
	}

	IFluentXmlWriterComplex IFluentXmlWriterSimple.EndElem()
	{
		_xmlWriter.WriteEndElement(); // ending the simple element
		_xmlWriter.WriteFullEndElement(); // ending the complex element
		return this;
	}

	IFluentXmlWriterSimple IFluentXmlWriterSimple.Simple(string name)
	{
		_xmlWriter.WriteEndElement(); // ending the simple element
		_xmlWriter.WriteStartElement(name); // starting a new simple element
		return this;
	}

	IFluentXmlWriterSimple IFluentXmlWriterSimple.ManySimple(params SimpleElement[] simpleElements)
	{
		if (simpleElements != null)
		{
			foreach (var simpleElem in simpleElements)
			{
				((IFluentXmlWriterSimple)this).Simple(simpleElem.Name);
				foreach ((string key, string value) in simpleElem.Attributes)
				{
					((IFluentXmlWriterSimple)this).Attr(key, value);
				}
			}
		}

		return this;
	}

	IFluentXmlWriterComplex IFluentXmlWriterSimple.Text(string text)
	{
		_xmlWriter.WriteEndElement(); // ends the simple element
		_xmlWriter.WriteString(text);
		return this;
	}

	IFluentXmlWriterComplex IFluentXmlWriterSimple.CData(string text)
	{
		_xmlWriter.WriteEndElement(); // ends the simple element
		_xmlWriter.WriteCData(text);
		return this;
	}

	IFluentXmlWriterComplex IFluentXmlWriterSimple.Comment(string comment)
	{
		_xmlWriter.WriteEndElement(); // ends the simple element
		_xmlWriter.WriteComment(comment);
		return this;
	}

	void IFluentXmlWriterSimple.OutputToString(Action<string> action)
	{
		_xmlWriter.WriteEndElement(); // ends the simple element
		_xmlWriter.WriteEndElement(); // Top-level element
		action(_stringBuilder.ToString());
	}

	void IFluentXmlWriterSimple.OutputToFile(string fileName)
	{
		_xmlWriter.WriteEndElement(); // ends the simple element
		_xmlWriter.WriteEndElement(); // Top-level element
		File.WriteAllText(fileName, _stringBuilder.ToString());
	}
}
