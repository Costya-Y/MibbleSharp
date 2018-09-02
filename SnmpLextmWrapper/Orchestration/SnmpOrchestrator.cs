using System;
using System.Collections.Generic;
using SnmpLextmWrapper.Domain;

namespace SnmpLextmWrapper.Orchestration
{
    public class SnmpOrchestrator
    {
        private readonly List<string> oidList = new List<string>
        {
            "1.3.6.1.2.1.1", "1.0.8802.1.1.2.1", "1.2.840.10006.300.43.1.1", "1.2.840.10006.300.43.1.2.1",
            "1.3.6.1.2.1.10.7.2", "1.3.6.1.2.1.26.4", "1.3.6.1.2.1.26.5",
            "1.3.6.1.2.1.2.2.1", "1.3.6.1.2.1.25.6.3.1", "1.3.6.1.2.1.31",
            "1.3.6.1.2.1.4.20", "1.3.6.1.2.1.4.28", "1.3.6.1.2.1.4.30",
            "1.3.6.1.2.1.47.1.1", "1.3.6.1.2.1.47.1.3.2", "1.3.6.1.4.1.9"
        };

        public void StartRecording(ISnmpParameters param)
        {
            foreach (var oid in oidList)
            foreach (var line in new SnmpWalk().ExecuteRequest(param, oid))
                Console.WriteLine(line.ToString());
        }

        public SnmpLine Get(ISnmpParameters param, string oid)
        {
            var result = new SnmpGet().ExecuteRequest(param, oid);
            return result[0];
        }

        public SnmpLine GetNext(ISnmpParameters param, string oid)
        {
            var result = new SnmpGetNext().ExecuteRequest(param, oid);
            return result[0];
        }

        public SnmpTable GetTable(ISnmpParameters param, string oid, Dictionary<string, string> columns)
        {
            var result = new SnmpWalk().ExecuteRequest(param, oid, true);
            return new SnmpTable(result, columns);
        }

        public List<SnmpLine> Walk(ISnmpParameters param, string oid)
        {
            var result = new SnmpGetNext().ExecuteRequest(param, oid);
            return result;
        }

        public List<SnmpLine> GetBulk(ISnmpParameters param, string oid)
        {
            var result = new SnmpGetNext().ExecuteRequest(param, oid);
            return result;
        }
    }
}