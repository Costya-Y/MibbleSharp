using System.Collections.Generic;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using SnmpLextmWrapper.Domain;

namespace SnmpLextmWrapper.Orchestration
{
    public class SnmpWalk : ISnmpMethodCaller
    {
        public List<SnmpLine> ExecuteRequest(ISnmpParameters snmpParams, string OidLine)
        {
            var community = new OctetString("");
            if (snmpParams.GetType() == typeof(SnmpV2Parameters))
            {
                community = ((SnmpV2Parameters)snmpParams).SnmpCommunity;
            }

            var result = new List<Variable>();
            Messenger.Walk(snmpParams.Version,
                snmpParams.IP,
                community,
                new ObjectIdentifier(OidLine),
                result,
                60000,
                WalkMode.Default);
            return SnmpHelper.ConvertOutputToSnmpLine(result);
        }

        public List<SnmpLine> ExecuteRequest(ISnmpParameters snmpParams, string OidLine, bool DoSubTreeOnly)
        {
            var community = new OctetString("");
            if (snmpParams.GetType() == typeof(SnmpV2Parameters))
            {
                community = ((SnmpV2Parameters)snmpParams).SnmpCommunity;
            }

            var result = new List<Variable>();
            Messenger.Walk(snmpParams.Version,
                snmpParams.IP,
                community,
                new ObjectIdentifier(OidLine),
                result,
                60000,
                WalkMode.WithinSubtree);
            return SnmpHelper.ConvertOutputToSnmpLine(result);
        }
    }
}