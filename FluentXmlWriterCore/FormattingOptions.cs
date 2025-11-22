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
		return new FormattingOptions
		{
			NewLine = newLine,
			IndentChar = this.IndentChar,
			Indentation = this.Indentation,
			Indent = true,
			NewLineOnAttributes = this.NewLineOnAttributes
		};
	}

	public FormattingOptions WithTabs(int indentation = 1)
	{
		return new FormattingOptions
		{
			IndentChar = '\t',
			Indentation = indentation,
			Indent = true,
			NewLine = this.NewLine,
			NewLineOnAttributes = this.NewLineOnAttributes
		};
	}

	public FormattingOptions WithSpaces(int indentation = 4)
	{
		return new FormattingOptions
		{
			IndentChar = ' ',
			Indentation = indentation,
			Indent = true,
			NewLine = this.NewLine,
			NewLineOnAttributes = this.NewLineOnAttributes
		};
	}

	public FormattingOptions WithAttributesOnNewLine(bool newLineOnAttributes)
	{
		return new FormattingOptions
		{
			NewLineOnAttributes = newLineOnAttributes,
			IndentChar = this.IndentChar,
			Indentation = this.Indentation,
			Indent = this.Indent,
			NewLine = this.NewLine
		};
	}
}
