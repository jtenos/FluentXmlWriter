using System;
using System.Collections.Generic;

namespace FluentXmlWriter
{
    public class SimpleElement
    {
        private readonly string _name;
        private readonly IList<Tuple<string, string>> _attributes = new List<Tuple<string, string>>();

        private SimpleElement(string name) 
        {
            _name = name;
        }

        public static SimpleElement Create(string name)
        {
            return new SimpleElement(name);
        }

        public SimpleElement Attr(string name, string value)
        {
            _attributes.Add(new Tuple<string, string>(name, value));
            return this;
        }

        internal IList<Tuple<string, string>> Attributes { get { return _attributes; } }
        internal string Name { get { return _name; } }
    }
}
