using System;
using System.Collections.Generic;
using SnmpLextmWrapper.Domain;

namespace SnmpLextmWrapper.Orchestration
{
    public class SnmpBulk : ISnmpMethodCaller
    {
        public List<SnmpLine> ExecuteRequest(ISnmpParameters SnmpHandler, string OidLine)
        {
            throw new NotImplementedException();
        }
    }
}