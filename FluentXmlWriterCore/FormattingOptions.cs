namespace FluentXmlWriterCore;

public struct FormattingOptions
{
	public string NewLine { get; private set; }
	public char IndentChar { get; private set; }
	public int Indentation { get; private set; }
	public bool Indent { get; private set; }
	public bool NewLineOnAttributes { get; private set; }

	public FormattingOptions()
	{
		NewLine = string.Empty;
		IndentChar = '\0';
		Indentation = 0;
		Indent = false;
		NewLineOnAttributes = false;
	}

	public static FormattingOptions Default => new();

	public FormattingOptions WithNewLine(string newLine)
	{
		NewLine = newLine;
		Indent = true;
		return this;
	}

	public FormattingOptions WithTabs(int indentation = 1)
	{
		IndentChar = '\t';
		Indentation = indentation;
		Indent = true;
		return this;
	}

	public FormattingOptions WithSpaces(int indentation = 4)
	{
		IndentChar = ' ';
		Indentation = indentation;
		Indent = true;
		return this;
	}

	public FormattingOptions WithAttributesOnNewLine(bool newLineOnAttributes)
	{
		NewLineOnAttributes = newLineOnAttributes;
		return this;
	}
}
