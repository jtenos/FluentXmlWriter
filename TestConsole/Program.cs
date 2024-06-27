using System;

namespace FluentXmlWriter.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            FluentXmlWriter.Start("root")
                .Simple("First")
                .Simple("Second").Attr("a", "b")
                .Complex("Arr")
                    .ManySimple(
                        SimpleElement.Create("Arr1").Attr("key1", "val1"),
                        SimpleElement.Create("Arr2").Attr("key2", "val2")
                    ).EndElem()
                .Complex("Third").Text("My Text").EndElem()
                .Complex("Fourth").CData("Hello there!").EndElem()
                .Comment("A comment!")
                .OutputToString(Console.WriteLine);
        }
    }
}
