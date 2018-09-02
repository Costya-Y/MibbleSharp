using System.Net;
using Lextm.SharpSnmpLib;

namespace SnmpLextmWrapper.Domain
{
    public interface ISnmpParameters
    {
        VersionCode Version { get; }
        IPEndPoint IP { get; set; }
        void SetVersion(string version);
    }
}