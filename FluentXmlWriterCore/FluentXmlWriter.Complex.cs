namespace FluentXmlWriterCore;

partial class FluentXmlWriter
{
	IFluentXmlWriterComplex IFluentXmlWriterComplex.Attr(string name, string value)
	{
		_xmlWriter.WriteAttributeString(name, value);
		return this;
	}

	IFluentXmlWriterComplex IFluentXmlWriterComplex.Complex(string name)
	{
		_xmlWriter.WriteStartElement(name);
		return this;
	}

	IFluentXmlWriterComplex IFluentXmlWriterComplex.EndElem()
	{
		_xmlWriter.WriteFullEndElement();
		return this;
	}

	IFluentXmlWriterSimple IFluentXmlWriterComplex.Simple(string name)
	{
		_xmlWriter.WriteStartElement(name);
		return this;
	}

	IFluentXmlWriterSimple IFluentXmlWriterComplex.ManySimple(params SimpleElement[] simpleElements)
	{
		if (simpleElements != null)
		{
			for (int i = 0; i < simpleElements.Length; ++i)
			{
				if (i == 0)
				{
					((IFluentXmlWriterComplex)this).Simple(simpleElements[i].Name);
				}
				else
				{
					((IFluentXmlWriterSimple)this).Simple(simpleElements[i].Name);
				}
				foreach ((string key, string value) in simpleElements[i].Attributes)
				{
					((IFluentXmlWriterSimple)this).Attr(key, value);
				}
			}
		}

		return this;
	}

	IFluentXmlWriterComplex IFluentXmlWriterComplex.Text(string text)
	{
		_xmlWriter.WriteString(text);
		return this;
	}

	IFluentXmlWriterComplex IFluentXmlWriterComplex.CData(string text)
	{
		_xmlWriter.WriteCData(text);
		return this;
	}

	IFluentXmlWriterComplex IFluentXmlWriterComplex.Comment(string comment)
	{
		_xmlWriter.WriteComment(comment);
		return this;
	}

	void IFluentXmlWriterComplex.OutputToString(Action<string> action)
	{
		_xmlWriter.WriteEndElement(); // Top-level element
		action(_stringBuilder.ToString());
	}

	void IFluentXmlWriterComplex.OutputToFile(string fileName)
	{
		_xmlWriter.WriteEndElement(); // Top-level element
		File.WriteAllText(fileName, _stringBuilder.ToString());
	}
}
