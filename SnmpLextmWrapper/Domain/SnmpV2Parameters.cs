using System.Net;
using Lextm.SharpSnmpLib;

namespace SnmpLextmWrapper.Domain
{
    public class SnmpV2Parameters : BaseSnmpParameters
    {
        public OctetString SnmpCommunity;

        public SnmpV2Parameters(IPAddress ip, int port, string snmpCommunity)
        {
            if (port == 0)
            {
                port = 161;
            }
            SnmpCommunity = new OctetString(snmpCommunity);
            IP = new IPEndPoint(ip, port);
            SetVersion("2");
        }
    }
}