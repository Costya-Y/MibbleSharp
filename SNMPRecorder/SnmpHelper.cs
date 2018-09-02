using System;
using System.Collections.Generic;
using System.Linq;
using Lextm.SharpSnmpLib;
using SNMPRecorder.Domain;

namespace SNMPRecorder
{
    public static class SnmpHelper
    {
        public static List<SnmpLine> ConvertOutputToSnmpLine(IEnumerable<Variable> snmpResponse)
        {
            var result = new List<SnmpLine>();
            foreach (var variable in snmpResponse)
            {
                SnmpLine line;
                line = new SnmpLine 
                {
                    Oid = variable.Id.ToString(), Type = variable.Data.TypeCode.ToString(),
                    Value = variable.Data.ToString()
                };
                if (line.Type == "OctetString")
                {
                    line.Value = BitConverter.ToString(variable.Data.ToBytes());
                }
            }

            return result;
        }
    }
}