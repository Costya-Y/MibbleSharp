using System.Collections.Generic;
using SnmpLextmWrapper.Domain;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;

namespace SnmpLextmWrapper.Orchestration
{
    internal class SnmpGet : ISnmpMethodCaller
    {
  
        public List<SnmpLine> ExecuteRequest(ISnmpParameters snmpParams, string OidLine)
        {
            var community = new OctetString("");
            if (snmpParams.GetType() == typeof(SnmpV2Parameters))
            {
                community = ((SnmpV2Parameters)snmpParams).SnmpCommunity;
            }
            var result = Messenger.Get(snmpParams.Version,
                           snmpParams.IP,
                           community,
                           new List<Variable> { new Variable(new ObjectIdentifier(OidLine)) },
                           60000);
            return SnmpHelper.ConvertOutputToSnmpLine(result);
        }
    }
}
