using System;
using System.Collections.Generic;
using System.Linq;
using Lextm.SharpSnmpLib;
using SnmpLextmWrapper.Domain;

namespace SnmpLextmWrapper
{
    public static class SnmpHelper
    {
        public static List<SnmpLine> ConvertOutputToSnmpLine(IEnumerable<Variable> snmpResponse)
        {
            return snmpResponse.Select(variable => new SnmpLine
            {
                Oid = variable.Id.ToString(), Type = variable.Data.TypeCode.ToString(), Value = variable.Data.ToString()
            }).ToList();
        }
    }
}