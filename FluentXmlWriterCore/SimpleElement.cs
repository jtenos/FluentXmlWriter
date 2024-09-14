namespace FluentXmlWriterCore;

public class SimpleElement(string name)
{
	private readonly List<KeyValuePair<string, string>> _attributes = [];

	public static SimpleElement Create(string name) => new(name);

	public SimpleElement Attr(string name, string value)
	{
		_attributes.Add(new(name, value));
		return this;
	}

	internal IList<KeyValuePair<string, string>> Attributes => _attributes;
	internal string Name => name;
}
