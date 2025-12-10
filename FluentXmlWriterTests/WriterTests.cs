using FluentXmlWriterCore;

namespace FluentXmlWriterTests;

[TestClass]
public class WriterTests
{
	[TestMethod]
	public void TestSimpleElementsWithAttributes()
	{
			string xml = null!;

			FluentXmlWriter.Start("top", indented: true)
				.ManySimple(
					SimpleElement.Create("a").Attr("at1", "val1").Attr("at2", "val2")
					, SimpleElement.Create("b")
					, SimpleElement.Create("c").Attr("at3", "val3")
				)
				.OutputToString(x => xml = x);

			const string expected = """
				<top>
					<a at1="val1" at2="val2" />
					<b />
					<c at3="val3" />
				</top>
				""";
			Assert.AreEqual(expected, xml);
	}

	[TestMethod]
	public void TestAppConfigStructure()
	{
		string xml = null!;

		FluentXmlWriter.Start("configuration", indented: true)
			.Complex("appSettings")
				.ManySimple(
					SimpleElement.Create("add").Attr("key", "DatabaseConnection").Attr("value", "Server=localhost;Database=mydb"),
					SimpleElement.Create("add").Attr("key", "Timeout").Attr("value", "30"),
					SimpleElement.Create("add").Attr("key", "EnableLogging").Attr("value", "true")
				)
				.EndElem()
			.Complex("connectionStrings")
				.ManySimple(
					SimpleElement.Create("add").Attr("name", "DefaultConnection")
						.Attr("connectionString", "Data Source=.;Initial Catalog=MyDb;")
						.Attr("providerName", "System.Data.SqlClient")
				)
				.EndElem()
			.OutputToString(x => xml = x);

		const string expected = """
			<configuration>
				<appSettings>
					<add key="DatabaseConnection" value="Server=localhost;Database=mydb" />
					<add key="Timeout" value="30" />
					<add key="EnableLogging" value="true" />
				</appSettings>
				<connectionStrings>
					<add name="DefaultConnection" connectionString="Data Source=.;Initial Catalog=MyDb;" providerName="System.Data.SqlClient" />
				</connectionStrings>
			</configuration>
			""";
		Assert.AreEqual(expected, xml);
	}

	[TestMethod]
	public void TestRestApiResponseStructure()
	{
		string xml = null!;

		FluentXmlWriter.Start("response", indented: true)
			.Attr("status", "success")
			.Complex("metadata")
				.Complex("totalResults").Text("150").EndElem()
				.Complex("page").Text("1").EndElem()
				.Complex("pageSize").Text("10").EndElem()
				.EndElem()
			.Complex("data")
				.Complex("user")
					.Complex("id").Text("12345").EndElem()
					.Complex("name").Text("John Doe").EndElem()
					.Complex("email").Text("john@example.com").EndElem()
					.Complex("address")
						.Complex("street").Text("123 Main St").EndElem()
						.Complex("city").Text("New York").EndElem()
						.Complex("zip").Text("10001").EndElem()
						.EndElem()
					.EndElem()
				.Complex("user")
					.Complex("id").Text("67890").EndElem()
					.Complex("name").Text("Jane Smith").EndElem()
					.Complex("email").Text("jane@example.com").EndElem()
					.Complex("address")
						.Complex("street").Text("456 Oak Ave").EndElem()
						.Complex("city").Text("Boston").EndElem()
						.Complex("zip").Text("02101").EndElem()
						.EndElem()
					.EndElem()
				.EndElem()
			.OutputToString(x => xml = x);

		const string expected = """
			<response status="success">
				<metadata>
					<totalResults>150</totalResults>
					<page>1</page>
					<pageSize>10</pageSize>
				</metadata>
				<data>
					<user>
						<id>12345</id>
						<name>John Doe</name>
						<email>john@example.com</email>
						<address>
							<street>123 Main St</street>
							<city>New York</city>
							<zip>10001</zip>
						</address>
					</user>
					<user>
						<id>67890</id>
						<name>Jane Smith</name>
						<email>jane@example.com</email>
						<address>
							<street>456 Oak Ave</street>
							<city>Boston</city>
							<zip>02101</zip>
						</address>
					</user>
				</data>
			</response>
			""";
		Assert.AreEqual(expected, xml);
	}

	[TestMethod]
	public void TestSoapEnvelopeStructure()
	{
		string xml = null!;

		FluentXmlWriter.Start("soap:Envelope", indented: true)
			.Attr("xmlns:soap", "http://schemas.xmlsoap.org/soap/envelope/")
			.Attr("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance")
			.Complex("soap:Header")
				.Complex("AuthToken").Text("abc123xyz789").EndElem()
				.EndElem()
			.Complex("soap:Body")
				.Complex("GetUserRequest")
					.Attr("xmlns", "http://example.com/api")
					.Complex("UserId").Text("12345").EndElem()
					.Complex("IncludeDetails").Text("true").EndElem()
					.EndElem()
				.EndElem()
			.OutputToString(x => xml = x);

		const string expected = """
			<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
				<soap:Header>
					<AuthToken>abc123xyz789</AuthToken>
				</soap:Header>
				<soap:Body>
					<GetUserRequest xmlns="http://example.com/api">
						<UserId>12345</UserId>
						<IncludeDetails>true</IncludeDetails>
					</GetUserRequest>
				</soap:Body>
			</soap:Envelope>
			""";
		Assert.AreEqual(expected, xml);
	}

	[TestMethod]
	public void TestRssFeedStructure()
	{
		string xml = null!;

		FluentXmlWriter.Start("rss", indented: true)
			.Attr("version", "2.0")
			.Complex("channel")
				.Complex("title").Text("Tech Blog").EndElem()
				.Complex("link").Text("https://techblog.example.com").EndElem()
				.Complex("description").Text("Latest technology news and articles").EndElem()
				.Complex("item")
					.Complex("title").Text("Introduction to Fluent APIs").EndElem()
					.Complex("link").Text("https://techblog.example.com/fluent-apis").EndElem()
					.Complex("description").Text("Learn how to design fluent APIs in C#").EndElem()
					.Complex("pubDate").Text("Mon, 20 Nov 2025 10:00:00 GMT").EndElem()
					.EndElem()
				.Complex("item")
					.Complex("title").Text("XML Processing Best Practices").EndElem()
					.Complex("link").Text("https://techblog.example.com/xml-practices").EndElem()
					.Complex("description").Text("Tips for efficient XML processing").EndElem()
					.Complex("pubDate").Text("Tue, 21 Nov 2025 14:30:00 GMT").EndElem()
					.EndElem()
				.EndElem()
			.OutputToString(x => xml = x);

		const string expected = """
			<rss version="2.0">
				<channel>
					<title>Tech Blog</title>
					<link>https://techblog.example.com</link>
					<description>Latest technology news and articles</description>
					<item>
						<title>Introduction to Fluent APIs</title>
						<link>https://techblog.example.com/fluent-apis</link>
						<description>Learn how to design fluent APIs in C#</description>
						<pubDate>Mon, 20 Nov 2025 10:00:00 GMT</pubDate>
					</item>
					<item>
						<title>XML Processing Best Practices</title>
						<link>https://techblog.example.com/xml-practices</link>
						<description>Tips for efficient XML processing</description>
						<pubDate>Tue, 21 Nov 2025 14:30:00 GMT</pubDate>
					</item>
				</channel>
			</rss>
			""";
		Assert.AreEqual(expected, xml);
	}

	[TestMethod]
	public void TestComplexNestedStructure()
	{
		string xml = null!;

		FluentXmlWriter.Start("project", indented: true)
			.Attr("name", "MyApp")
			.Attr("version", "1.0")
			.Complex("dependencies")
				.Complex("dependency")
					.Complex("name").Text("Newtonsoft.Json").EndElem()
					.Complex("version").Text("13.0.3").EndElem()
					.EndElem()
				.Complex("dependency")
					.Complex("name").Text("Microsoft.Extensions.Logging").EndElem()
					.Complex("version").Text("8.0.0").EndElem()
					.EndElem()
				.EndElem()
			.Complex("buildConfiguration")
				.Complex("targetFramework").Text("net8.0").EndElem()
				.Complex("outputSettings")
					.Complex("outputPath").Text("bin/Release").EndElem()
					.Complex("optimize").Text("true").EndElem()
					.EndElem()
				.EndElem()
			.OutputToString(x => xml = x);

		const string expected = """
			<project name="MyApp" version="1.0">
				<dependencies>
					<dependency>
						<name>Newtonsoft.Json</name>
						<version>13.0.3</version>
					</dependency>
					<dependency>
						<name>Microsoft.Extensions.Logging</name>
						<version>8.0.0</version>
					</dependency>
				</dependencies>
				<buildConfiguration>
					<targetFramework>net8.0</targetFramework>
					<outputSettings>
						<outputPath>bin/Release</outputPath>
						<optimize>true</optimize>
					</outputSettings>
				</buildConfiguration>
			</project>
			""";
		Assert.AreEqual(expected, xml);
	}

	[TestMethod]
	public void TestCDataContent()
	{
		string xml = null!;

		FluentXmlWriter.Start("document", indented: true)
			.Complex("content")
				.Complex("script").CData("function test() { if (x < y && a > b) { return true; } }").EndElem()
				.EndElem()
			.OutputToString(x => xml = x);

		const string expected = """
			<document>
				<content>
					<script><![CDATA[function test() { if (x < y && a > b) { return true; } }]]></script>
				</content>
			</document>
			""";
		Assert.AreEqual(expected, xml);
	}

	[TestMethod]
	public void TestCommentsInXml()
	{
		string xml = null!;

		FluentXmlWriter.Start("config", indented: true)
			.Comment("Database configuration section")
			.Complex("database")
				.Complex("host").Text("localhost").EndElem()
				.Complex("port").Text("5432").EndElem()
				.EndElem()
			.Comment("Cache settings")
			.Complex("cache")
				.Complex("enabled").Text("true").EndElem()
				.EndElem()
			.OutputToString(x => xml = x);

		const string expected = """
			<config>
				<!--Database configuration section-->
				<database>
					<host>localhost</host>
					<port>5432</port>
				</database>
				<!--Cache settings-->
				<cache>
					<enabled>true</enabled>
				</cache>
			</config>
			""";
		Assert.AreEqual(expected, xml);
	}

	[TestMethod]
	public void TestMixedContent()
	{
		string xml = null!;

		FluentXmlWriter.Start("article", indented: true)
			.Complex("paragraph")
				.Text("This is the first part of text. ")
				.Complex("bold")
					.Text("This is bold text.")
					.EndElem()
				.Text(" And this continues after bold.")
				.EndElem()
			.OutputToString(x => xml = x);

		const string expected = """
			<article>
				<paragraph>This is the first part of text. <bold>This is bold text.</bold> And this continues after bold.</paragraph>
			</article>
			""";
		Assert.AreEqual(expected, xml);
	}

	[TestMethod]
	public void TestSvgStructure()
	{
		string xml = null!;

		FluentXmlWriter.Start("svg", indented: true)
			.Attr("xmlns", "http://www.w3.org/2000/svg")
			.Attr("width", "100")
			.Attr("height", "100")
			.ManySimple(
				SimpleElement.Create("circle")
					.Attr("cx", "50")
					.Attr("cy", "50")
					.Attr("r", "40")
					.Attr("fill", "red"),
				SimpleElement.Create("rect")
					.Attr("x", "10")
					.Attr("y", "10")
					.Attr("width", "30")
					.Attr("height", "30")
					.Attr("fill", "blue")
			)
			.OutputToString(x => xml = x);

		const string expected = """
			<svg xmlns="http://www.w3.org/2000/svg" width="100" height="100">
				<circle cx="50" cy="50" r="40" fill="red" />
				<rect x="10" y="10" width="30" height="30" fill="blue" />
			</svg>
			""";
		Assert.AreEqual(expected, xml);
	}

	[TestMethod]
	public void TestMavenPomStructure()
	{
		string xml = null!;

		FluentXmlWriter.Start("project", indented: true)
			.Attr("xmlns", "http://maven.apache.org/POM/4.0.0")
			.Complex("modelVersion").Text("4.0.0").EndElem()
			.Complex("groupId").Text("com.example").EndElem()
			.Complex("artifactId").Text("my-app").EndElem()
			.Complex("version").Text("1.0-SNAPSHOT").EndElem()
			.Complex("dependencies")
				.Complex("dependency")
					.Complex("groupId").Text("junit").EndElem()
					.Complex("artifactId").Text("junit").EndElem()
					.Complex("version").Text("4.13.2").EndElem()
					.Complex("scope").Text("test").EndElem()
					.EndElem()
				.EndElem()
			.OutputToString(x => xml = x);

		const string expected = """
			<project xmlns="http://maven.apache.org/POM/4.0.0">
				<modelVersion>4.0.0</modelVersion>
				<groupId>com.example</groupId>
				<artifactId>my-app</artifactId>
				<version>1.0-SNAPSHOT</version>
				<dependencies>
					<dependency>
						<groupId>junit</groupId>
						<artifactId>junit</artifactId>
						<version>4.13.2</version>
						<scope>test</scope>
					</dependency>
				</dependencies>
			</project>
			""";
		Assert.AreEqual(expected, xml);
	}

	[TestMethod]
	public void TestWebConfigStructure()
	{
		string xml = null!;

		FluentXmlWriter.Start("configuration", indented: true)
			.Complex("system.web")
				.Complex("compilation")
					.Attr("debug", "true")
					.Attr("targetFramework", "4.8")
					.EndElem()
				.Complex("httpRuntime")
					.Attr("targetFramework", "4.8")
					.EndElem()
				.EndElem()
			.Complex("system.webServer")
				.Complex("modules")
					.Attr("runAllManagedModulesForAllRequests", "true")
					.EndElem()
				.EndElem()
			.OutputToString(x => xml = x);

		const string expected = """
			<configuration>
				<system.web>
					<compilation debug="true" targetFramework="4.8">
					</compilation>
					<httpRuntime targetFramework="4.8">
					</httpRuntime>
				</system.web>
				<system.webServer>
					<modules runAllManagedModulesForAllRequests="true">
					</modules>
				</system.webServer>
			</configuration>
			""";
		Assert.AreEqual(expected, xml);
	}

	[TestMethod]
	public void TestXmlWithSpecialCharacters()
	{
		string xml = null!;

		FluentXmlWriter.Start("data", indented: true)
			.Complex("field")
				.Complex("value").Text("Less than < and greater than > symbols").EndElem()
				.EndElem()
			.Complex("field")
				.Complex("value").Text("Ampersand & and quotes \" test").EndElem()
				.EndElem()
			.OutputToString(x => xml = x);

		const string expected = """
			<data>
				<field>
					<value>Less than &lt; and greater than &gt; symbols</value>
				</field>
				<field>
					<value>Ampersand &amp; and quotes " test</value>
				</field>
			</data>
			""";
		Assert.AreEqual(expected, xml);
	}

	[TestMethod]
	public void TestEmptyElementsAndSelfClosing()
	{
		string xml = null!;

		FluentXmlWriter.Start("document", indented: true)
			.ManySimple(
				SimpleElement.Create("emptyTag1"),
				SimpleElement.Create("emptyTag2"),
				SimpleElement.Create("tagWithAttr").Attr("id", "123")
			)
			.OutputToString(x => xml = x);

		const string expected = """
			<document>
				<emptyTag1 />
				<emptyTag2 />
				<tagWithAttr id="123" />
			</document>
			""";
		Assert.AreEqual(expected, xml);
	}

	[TestMethod]
	public void TestMinifiedOutput_Default()
	{
		var xml = FluentXmlWriter.Start("root")
			.Simple("someElem").Attr("id", "1")
			.Complex("something")
				.Simple("else")
				.EndElem()
			.OutputToString();

		const string expected = "<root><someElem id=\"1\" /><something><else /></something></root>";
		Assert.AreEqual(expected, xml);
	}

	[TestMethod]
	public void TestMinifiedOutput_ExplicitFalse()
	{
		var xml = FluentXmlWriter.Start("root")
			.Complex("child")
				.Complex("grandchild").Text("value").EndElem()
				.EndElem()
			.OutputToString(indented: false);

		const string expected = "<root><child><grandchild>value</grandchild></child></root>";
		Assert.AreEqual(expected, xml);
	}

	[TestMethod]
	public void TestIndentedOutput_WithBoolParameter()
	{
		var xml = FluentXmlWriter.Start("root")
			.Complex("child")
				.Complex("grandchild").Text("value").EndElem()
				.EndElem()
			.OutputToString(indented: true);

		var expected = "<root>" + Environment.NewLine +
			"\t<child>" + Environment.NewLine +
			"\t\t<grandchild>value</grandchild>" + Environment.NewLine +
			"\t</child>" + Environment.NewLine +
			"</root>";
		Assert.AreEqual(expected, xml);
	}

	[TestMethod]
	public void TestFormattingOptions_WithSpaces()
	{
		var xml = FluentXmlWriter.Start("root")
			.Complex("child")
				.Complex("grandchild").Text("value").EndElem()
				.EndElem()
			.OutputToString(FormattingOptions.Default
				.WithSpaces(2)
				.WithNewLine(Environment.NewLine));

		var expected = "<root>" + Environment.NewLine +
			"  <child>" + Environment.NewLine +
			"    <grandchild>value</grandchild>" + Environment.NewLine +
			"  </child>" + Environment.NewLine +
			"</root>";
		Assert.AreEqual(expected, xml);
	}

	[TestMethod]
	public void TestFormattingOptions_WithSpaces4()
	{
		var xml = FluentXmlWriter.Start("root")
			.Complex("child")
				.Text("test")
				.EndElem()
			.OutputToString(FormattingOptions.Default
				.WithSpaces(4)
				.WithNewLine("\n"));

		const string expected = "<root>\n    <child>test</child>\n</root>";
		Assert.AreEqual(expected, xml);
	}

	[TestMethod]
	public void TestFormattingOptions_WithTabs()
	{
		var xml = FluentXmlWriter.Start("root")
			.Complex("child")
				.Complex("grandchild").Text("value").EndElem()
				.EndElem()
			.OutputToString(FormattingOptions.Default
				.WithTabs()
				.WithNewLine("\n"));

		const string expected = "<root>\n\t<child>\n\t\t<grandchild>value</grandchild>\n\t</child>\n</root>";
		Assert.AreEqual(expected, xml);
	}

	[TestMethod]
	public void TestFormattingOptions_WithWindowsLineEndings()
	{
		var xml = FluentXmlWriter.Start("root")
			.Complex("child").Text("value").EndElem()
			.OutputToString(FormattingOptions.Default
				.WithTabs()
				.WithNewLine("\r\n"));

		const string expected = "<root>\r\n\t<child>value</child>\r\n</root>";
		Assert.AreEqual(expected, xml);
	}

	[TestMethod]
	public void TestComplexStructure_Minified()
	{
		var xml = FluentXmlWriter.Start("response")
			.Attr("status", "success")
			.Complex("data")
				.Complex("user")
					.Complex("id").Text("123").EndElem()
					.Complex("name").Text("John").EndElem()
					.EndElem()
				.EndElem()
			.OutputToString();

		const string expected = "<response status=\"success\"><data><user><id>123</id><name>John</name></user></data></response>";
		Assert.AreEqual(expected, xml);
	}

	[TestMethod]
	public void TestSimpleElements_Minified()
	{
		var xml = FluentXmlWriter.Start("root")
			.ManySimple(
				SimpleElement.Create("a").Attr("x", "1"),
				SimpleElement.Create("b").Attr("y", "2"),
				SimpleElement.Create("c")
			)
			.OutputToString();

		const string expected = "<root><a x=\"1\" /><b y=\"2\" /><c /></root>";
		Assert.AreEqual(expected, xml);
	}

	[TestMethod]
	public void TestCData_Minified()
	{
		var xml = FluentXmlWriter.Start("root")
			.Complex("script")
				.CData("if (x < y) { return true; }")
				.EndElem()
			.OutputToString();

		const string expected = "<root><script><![CDATA[if (x < y) { return true; }]]></script></root>";
		Assert.AreEqual(expected, xml);
	}

	[TestMethod]
	public void TestComments_Minified()
	{
		var xml = FluentXmlWriter.Start("root")
			.Comment("This is a comment")
			.Complex("child").Text("value").EndElem()
			.OutputToString();

		const string expected = "<root><!--This is a comment--><child>value</child></root>";
		Assert.AreEqual(expected, xml);
	}

	[TestMethod]
	public void TestOutputToFile_WithIndentation()
	{
		var tempFile = Path.GetTempFileName();
		try
		{
			FluentXmlWriter.Start("root")
				.Complex("child").Text("value").EndElem()
				.OutputToFile(tempFile, indented: true);

			var xml = File.ReadAllText(tempFile);
			var expected = "<root>" + Environment.NewLine +
				"\t<child>value</child>" + Environment.NewLine +
				"</root>";
			Assert.AreEqual(expected, xml);
		}
		finally
		{
			File.Delete(tempFile);
		}
	}

	[TestMethod]
	public void TestOutputToFile_WithFormattingOptions()
	{
		var tempFile = Path.GetTempFileName();
		try
		{
			FluentXmlWriter.Start("root")
				.Complex("child").Text("value").EndElem()
				.OutputToFile(tempFile, FormattingOptions.Default
					.WithSpaces(4)
					.WithNewLine("\n"));

			var xml = File.ReadAllText(tempFile);
			const string expected = "<root>\n    <child>value</child>\n</root>";
			Assert.AreEqual(expected, xml);
		}
		finally
		{
			File.Delete(tempFile);
		}
	}

	[TestMethod]
	public void TestOutputToFile_Minified()
	{
		var tempFile = Path.GetTempFileName();
		try
		{
			FluentXmlWriter.Start("root")
				.Complex("child").Text("value").EndElem()
				.OutputToFile(tempFile);

			var xml = File.ReadAllText(tempFile);
			const string expected = "<root><child>value</child></root>";
			Assert.AreEqual(expected, xml);
		}
		finally
		{
			File.Delete(tempFile);
		}
	}
}
