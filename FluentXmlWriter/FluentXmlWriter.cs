using System;
using System.Text;
using System.Xml;
using System.IO;

namespace FluentXmlWriter
{
    public partial class FluentXmlWriter
        : IDisposable, IFluentXmlWriterComplex, IFluentXmlWriterSimple
    {
        private readonly StringBuilder _stringBuilder;
        private readonly StringWriter _stringWriter;
        private readonly XmlWriter _xmlWriter;

        private FluentXmlWriter(FluentXmlWriter writer)
        {
            if (writer == null)
            {
                _stringBuilder = new StringBuilder();
                _stringWriter = new StringWriter(_stringBuilder);
                _xmlWriter = new CustomXmlWriter(_stringWriter);
            }
            else
            {
                _stringBuilder = writer._stringBuilder;
                _stringWriter = writer._stringWriter;
                _xmlWriter = writer._xmlWriter;
            }
        }

        public static IFluentXmlWriterComplex Start(string topLevelElement)
        {
            IFluentXmlWriterComplex fluentXmlWriter = new FluentXmlWriter(null);
            return fluentXmlWriter.Complex(topLevelElement);
        }

        public override string ToString()
        {
            return _stringBuilder.ToString();
        }

        void IDisposable.Dispose()
        {
            ((IDisposable)_xmlWriter).Dispose();
        }
    }
}
