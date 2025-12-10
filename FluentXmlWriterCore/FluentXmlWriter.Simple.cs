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
		if (_isStreamMode)
		{
			throw new InvalidOperationException("OutputToString() cannot be called when using stream-based mode. Use Done() instead.");
		}
		_xmlWriter.WriteEndElement(); // ends the simple element
		_xmlWriter.WriteEndElement(); // Top-level element
		action(_stringBuilder!.ToString());
	}

	string IFluentXmlWriterSimple.OutputToString()
	{
		if (_isStreamMode)
		{
			throw new InvalidOperationException("OutputToString() cannot be called when using stream-based mode. Use Done() instead.");
		}
		_xmlWriter.WriteEndElement(); // ends the simple element
		_xmlWriter.WriteEndElement(); // Top-level element
		return _stringBuilder!.ToString();
	}

	string IFluentXmlWriterSimple.OutputToString(bool indented)
	{
		if (_isStreamMode)
		{
			throw new InvalidOperationException("OutputToString() cannot be called when using stream-based mode. Use Done() instead.");
		}
		_xmlWriter.WriteEndElement(); // ends the simple element
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

	string IFluentXmlWriterSimple.OutputToString(FormattingOptions options)
	{
		if (_isStreamMode)
		{
			throw new InvalidOperationException("OutputToString() cannot be called when using stream-based mode. Use Done() instead.");
		}
		_xmlWriter.WriteEndElement(); // ends the simple element
		_xmlWriter.WriteEndElement(); // Top-level element
		var xml = _stringBuilder!.ToString();
		
		if (!options.Indent)
		{
			return xml;
		}
		
		return ReformatXml(xml, options);
	}

	void IFluentXmlWriterSimple.OutputToFile(string fileName)
	{
		if (_isStreamMode)
		{
			throw new InvalidOperationException("OutputToFile() cannot be called when using stream-based mode. Use Done() instead.");
		}
		_xmlWriter.WriteEndElement(); // ends the simple element
		_xmlWriter.WriteEndElement(); // Top-level element
		File.WriteAllText(fileName, _stringBuilder!.ToString());
	}

	void IFluentXmlWriterSimple.OutputToFile(string fileName, bool indented)
	{
		if (_isStreamMode)
		{
			throw new InvalidOperationException("OutputToFile() cannot be called when using stream-based mode. Use Done() instead.");
		}
		var output = ((IFluentXmlWriterSimple)this).OutputToString(indented);
		File.WriteAllText(fileName, output);
	}

	void IFluentXmlWriterSimple.OutputToFile(string fileName, FormattingOptions options)
	{
		if (_isStreamMode)
		{
			throw new InvalidOperationException("OutputToFile() cannot be called when using stream-based mode. Use Done() instead.");
		}
		var output = ((IFluentXmlWriterSimple)this).OutputToString(options);
		File.WriteAllText(fileName, output);
	}

	string IFluentXmlWriterSimple.WriteToString()
	{
		return ((IFluentXmlWriterSimple)this).OutputToString();
	}

	string IFluentXmlWriterSimple.WriteToString(bool indented)
	{
		return ((IFluentXmlWriterSimple)this).OutputToString(indented);
	}

	string IFluentXmlWriterSimple.WriteToString(FormattingOptions options)
	{
		return ((IFluentXmlWriterSimple)this).OutputToString(options);
	}

	void IFluentXmlWriterSimple.Done()
	{
		if (!_isStreamMode)
		{
			throw new InvalidOperationException("Done() can only be called when using stream-based mode. Use OutputToString() or OutputToFile() instead.");
		}
		
		if (_isDone)
		{
			throw new InvalidOperationException("Done() has already been called on this FluentXmlWriter instance.");
		}
		
		_xmlWriter.WriteEndElement(); // ends the simple element
		_xmlWriter.WriteEndElement(); // Top-level element
		_xmlWriter.Flush();
		_textWriter.Flush();
		
		// Only dispose the TextWriter if we created it (from a Stream)
		if (_ownsTextWriter)
		{
			_textWriter.Dispose();
		}
		
		_isDone = true;
	}
}
