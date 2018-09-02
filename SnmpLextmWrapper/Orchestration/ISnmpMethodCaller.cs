using System.Collections.Generic;
using SnmpLextmWrapper.Domain;

namespace SnmpLextmWrapper.Orchestration
{
    public interface ISnmpMethodCaller
    {
        List<SnmpLine> ExecuteRequest(ISnmpParameters snmpParams, string OidLine);
    }
}