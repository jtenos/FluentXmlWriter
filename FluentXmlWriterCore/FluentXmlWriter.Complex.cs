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
		if (_isStreamMode)
		{
			throw new InvalidOperationException("OutputToString() cannot be called when using stream-based mode. Use Done() instead.");
		}
		_xmlWriter.WriteEndElement(); // Top-level element
		action(_stringBuilder!.ToString());
	}

	string IFluentXmlWriterComplex.OutputToString()
	{
		if (_isStreamMode)
		{
			throw new InvalidOperationException("OutputToString() cannot be called when using stream-based mode. Use Done() instead.");
		}
		_xmlWriter.WriteEndElement(); // Top-level element
		return _stringBuilder!.ToString();
	}

	string IFluentXmlWriterComplex.OutputToString(bool indented)
	{
		if (_isStreamMode)
		{
			throw new InvalidOperationException("OutputToString() cannot be called when using stream-based mode. Use Done() instead.");
		}
		_xmlWriter.WriteEndElement(); // Top-level element
		var xml = _stringBuilder!.ToString();
		
		if (!indented)
		{
			return xml;
		}
		
		var options = FormattingOptions.Default
			.WithTabs()
			.WithNewLine(Environment.NewLine);
		return ReformatXml(xml, options);
	}

	string IFluentXmlWriterComplex.OutputToString(FormattingOptions options)
	{
		if (_isStreamMode)
		{
			throw new InvalidOperationException("OutputToString() cannot be called when using stream-based mode. Use Done() instead.");
		}
		_xmlWriter.WriteEndElement(); // Top-level element
		var xml = _stringBuilder!.ToString();
		
		if (!options.Indent)
		{
			return xml;
		}
		
		return ReformatXml(xml, options);
	}

	void IFluentXmlWriterComplex.OutputToFile(string fileName)
	{
		if (_isStreamMode)
		{
			throw new InvalidOperationException("OutputToFile() cannot be called when using stream-based mode. Use Done() instead.");
		}
		_xmlWriter.WriteEndElement(); // Top-level element
		File.WriteAllText(fileName, _stringBuilder!.ToString());
	}

	void IFluentXmlWriterComplex.OutputToFile(string fileName, bool indented)
	{
		if (_isStreamMode)
		{
			throw new InvalidOperationException("OutputToFile() cannot be called when using stream-based mode. Use Done() instead.");
		}
		var output = ((IFluentXmlWriterComplex)this).OutputToString(indented);
		File.WriteAllText(fileName, output);
	}

	void IFluentXmlWriterComplex.OutputToFile(string fileName, FormattingOptions options)
	{
		if (_isStreamMode)
		{
			throw new InvalidOperationException("OutputToFile() cannot be called when using stream-based mode. Use Done() instead.");
		}
		var output = ((IFluentXmlWriterComplex)this).OutputToString(options);
		File.WriteAllText(fileName, output);
	}

	string IFluentXmlWriterComplex.WriteToString()
	{
		return ((IFluentXmlWriterComplex)this).OutputToString();
	}

	string IFluentXmlWriterComplex.WriteToString(bool indented)
	{
		return ((IFluentXmlWriterComplex)this).OutputToString(indented);
	}

	string IFluentXmlWriterComplex.WriteToString(FormattingOptions options)
	{
		return ((IFluentXmlWriterComplex)this).OutputToString(options);
	}

	void IFluentXmlWriterComplex.Done()
	{
		if (!_isStreamMode)
		{
			throw new InvalidOperationException("Done() can only be called when using stream-based mode. Use OutputToString() or OutputToFile() instead.");
		}
		
		if (_isDone)
		{
			throw new InvalidOperationException("Done() has already been called on this FluentXmlWriter instance.");
		}
		
		_xmlWriter.WriteEndElement(); // Top-level element
		_xmlWriter.Flush();
		_textWriter.Flush();
		_textWriter.Dispose();
		_isDone = true;
	}
}
