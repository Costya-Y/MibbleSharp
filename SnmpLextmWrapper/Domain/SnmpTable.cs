using System;
using System.Collections.Generic;
using System.Text;

namespace SnmpLextmWrapper.Domain
{
    public class SnmpTable
    {
        public Dictionary<string, Dictionary<string, string>> SnmpTableDict = new Dictionary<string, Dictionary<string, string>> ();

        public SnmpTable(List<SnmpLine> snmpLineList, Dictionary<string, string> columns)
        {
            foreach (var line in snmpLineList)
            {
                foreach (var column in columns)
                {
                    var columnOid = string.Format("{0}.", column.Value);
                    if (line.Oid.StartsWith(columnOid))
                    {
                        var index = line.Oid.Replace(columnOid, "");
                        if (!SnmpTableDict.ContainsKey(index))
                        {
                            SnmpTableDict[index] = new Dictionary<string, string>();
                        }

                        SnmpTableDict[index][column.Key] = line.Value;
                        break;
                    }
                }
            }
        }
    }

}
