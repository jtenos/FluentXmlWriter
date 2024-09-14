using FluentXmlWriterCore;

namespace FluentXmlWriterTests;

[TestClass]
public class WriterTests
{
	[TestMethod]
	public void TestMethod1()
	{
			string xml = null;

			FluentXmlWriter.Start("top")
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
}
