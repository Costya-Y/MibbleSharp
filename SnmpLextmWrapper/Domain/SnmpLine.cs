using System.Collections.Generic;

namespace SnmpLextmWrapper.Domain
{
    public class SnmpLine
    {
        public string Oid { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}", Oid, Type, Value);
        }
        
        public List<string> ToList()
        {
            return new List<string>() { Oid, Value, Type};
        }
    }
}