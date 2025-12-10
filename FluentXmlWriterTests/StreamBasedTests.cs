using System.Text;
using FluentXmlWriterCore;

namespace FluentXmlWriterTests;

[TestClass]
public class StreamBasedTests
{
	[TestMethod]
	public void TestStreamBasedWithTextWriter()
	{
		using var stringWriter = new StringWriter();
		
		FluentXmlWriter.Start(stringWriter, "top", FormattingOptions.Default)
			.ManySimple(
				SimpleElement.Create("a").Attr("id", "1"),
				SimpleElement.Create("b").Attr("id", "2")
			)
			.Done();

		var xml = stringWriter.ToString();
		const string expected = "<top><a id=\"1\" /><b id=\"2\" /></top>";
		Assert.AreEqual(expected, xml);
	}

	[TestMethod]
	public void TestStreamBasedWithTextWriter_Indented()
	{
		using var stringWriter = new StringWriter();
		var options = FormattingOptions.Default
			.WithTabs()
			.WithNewLine(Environment.NewLine);
		
		FluentXmlWriter.Start(stringWriter, "top", options)
			.ManySimple(
				SimpleElement.Create("a").Attr("id", "1"),
				SimpleElement.Create("b").Attr("id", "2")
			)
			.Done();

		var xml = stringWriter.ToString();
		var expected = "<top>" + Environment.NewLine +
			"\t<a id=\"1\" />" + Environment.NewLine +
			"\t<b id=\"2\" />" + Environment.NewLine +
			"</top>";
		Assert.AreEqual(expected, xml);
	}

	[TestMethod]
	public void TestStreamBasedWithFileStream()
	{
		var tempFile = Path.GetTempFileName();
		try
		{
			using (var fileStream = File.Create(tempFile))
			{
				FluentXmlWriter.Start(fileStream, "root", FormattingOptions.Default)
					.Complex("child").Text("value").EndElem()
					.Done();
			}

			var xml = File.ReadAllText(tempFile);
			const string expected = "<root><child>value</child></root>";
			Assert.AreEqual(expected, xml);
		}
		finally
		{
			File.Delete(tempFile);
		}
	}

	[TestMethod]
	public void TestStreamBasedWithFileStream_Indented()
	{
		var tempFile = Path.GetTempFileName();
		try
		{
			var options = FormattingOptions.Default
				.WithSpaces(2)
				.WithNewLine("\n");
			
			using (var fileStream = File.Create(tempFile))
			{
				FluentXmlWriter.Start(fileStream, "root", options)
					.Complex("child").Text("value").EndElem()
					.Done();
			}

			var xml = File.ReadAllText(tempFile);
			const string expected = "<root>\n  <child>value</child>\n</root>";
			Assert.AreEqual(expected, xml);
		}
		finally
		{
			File.Delete(tempFile);
		}
	}

	[TestMethod]
	public void TestStreamBasedWithFileStream_UTF8Encoding()
	{
		var tempFile = Path.GetTempFileName();
		try
		{
			using (var fileStream = File.Create(tempFile))
			{
				FluentXmlWriter.Start(fileStream, "root", FormattingOptions.Default, Encoding.UTF8)
					.Complex("child").Text("Hello 世界").EndElem()
					.Done();
			}

			var xml = File.ReadAllText(tempFile, Encoding.UTF8);
			const string expected = "<root><child>Hello 世界</child></root>";
			Assert.AreEqual(expected, xml);
		}
		finally
		{
			File.Delete(tempFile);
		}
	}

	[TestMethod]
	public void TestStreamMode_CannotCallOutputToString()
	{
		using var stringWriter = new StringWriter();
		
		var writer = FluentXmlWriter.Start(stringWriter, "top", FormattingOptions.Default);

		Assert.ThrowsException<InvalidOperationException>(() => writer.OutputToString());
	}

	[TestMethod]
	public void TestStreamMode_CannotCallOutputToFile()
	{
		using var stringWriter = new StringWriter();
		
		var writer = FluentXmlWriter.Start(stringWriter, "top", FormattingOptions.Default);

		Assert.ThrowsException<InvalidOperationException>(() => writer.OutputToFile("test.xml"));
	}

	[TestMethod]
	public void TestNormalMode_CannotCallDone()
	{
		var writer = FluentXmlWriter.Start("top");

		Assert.ThrowsException<InvalidOperationException>(() => writer.Done());
	}

	[TestMethod]
	public void TestWriteToString_BasicUsage()
	{
		var xml = FluentXmlWriter.Start("root")
			.Complex("child").Text("value").EndElem()
			.WriteToString();

		const string expected = "<root><child>value</child></root>";
		Assert.AreEqual(expected, xml);
	}

	[TestMethod]
	public void TestWriteToString_WithIndentation()
	{
		var xml = FluentXmlWriter.Start("root")
			.Complex("child").Text("value").EndElem()
			.WriteToString(indented: true);

		var expected = "<root>" + Environment.NewLine +
			"\t<child>value</child>" + Environment.NewLine +
			"</root>";
		Assert.AreEqual(expected, xml);
	}

	[TestMethod]
	public void TestWriteToString_WithFormattingOptions()
	{
		var xml = FluentXmlWriter.Start("root")
			.Complex("child").Text("value").EndElem()
			.WriteToString(FormattingOptions.Default
				.WithSpaces(4)
				.WithNewLine("\n"));

		const string expected = "<root>\n    <child>value</child>\n</root>";
		Assert.AreEqual(expected, xml);
	}

	[TestMethod]
	public void TestWriteToString_WithSimpleElements()
	{
		var xml = FluentXmlWriter.Start("root")
			.ManySimple(
				SimpleElement.Create("a").Attr("id", "1"),
				SimpleElement.Create("b").Attr("id", "2")
			)
			.WriteToString();

		const string expected = "<root><a id=\"1\" /><b id=\"2\" /></root>";
		Assert.AreEqual(expected, xml);
	}

	[TestMethod]
	public void TestStreamBasedWithComplexStructure()
	{
		using var stringWriter = new StringWriter();
		var options = FormattingOptions.Default
			.WithSpaces(2)
			.WithNewLine("\n");
		
		FluentXmlWriter.Start(stringWriter, "response", options)
			.Attr("status", "success")
			.Complex("data")
				.Complex("user")
					.Complex("id").Text("123").EndElem()
					.Complex("name").Text("John").EndElem()
					.EndElem()
				.EndElem()
			.Done();

		var xml = stringWriter.ToString();
		const string expected = "<response status=\"success\">\n  <data>\n    <user>\n      <id>123</id>\n      <name>John</name>\n    </user>\n  </data>\n</response>";
		Assert.AreEqual(expected, xml);
	}

	[TestMethod]
	public void TestStreamMode_CannotCallDoneTwice()
	{
		using var stringWriter = new StringWriter();
		
		var writer = FluentXmlWriter.Start(stringWriter, "top", FormattingOptions.Default);
		writer.Done();

		Assert.ThrowsException<InvalidOperationException>(() => writer.Done());
	}
}
