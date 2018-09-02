using System.Net;
using Lextm.SharpSnmpLib;

namespace SnmpLextmWrapper.Domain
{
    public class BaseSnmpParameters : ISnmpParameters
    {
        private VersionCode _version;
        public VersionCode Version { get { return _version; } }
        public IPEndPoint IP { get; set; }

        public void SetVersion(string version)
        {
            if (version.Contains("3"))
            {
                _version = VersionCode.V3;
            }
            else if (version.Contains("1"))
            {
                _version = VersionCode.V1;
            }
            else
            {
                _version = VersionCode.V2;
            }
        }
    }
}