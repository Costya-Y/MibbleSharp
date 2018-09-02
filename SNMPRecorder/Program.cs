using System;
using SNMPRecorder.Domain;
using SNMPRecorder.Orchestration;

namespace SNMPRecorder
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var ip = "";
            var community = "";
            foreach (var arg in args)
                if (arg.Contains("ip"))
                    ip = arg.Replace("--ip=", "");
                else if (arg.Contains("community")) community = arg.Replace("--community=", "");

            var snmpParams = new SnmpV2Parameters(ip, community);
            new SnmpOrchestrator().StartRecording(snmpParams);
            Console.WriteLine("Completed!");
        }
    }
}