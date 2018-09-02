using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Security;

namespace SnmpLextmWrapper.Domain
{
    public class SnmpV3Parameters : BaseSnmpParameters
    {
        public SnmpV3Parameters(string password, string privateKey, string userName, string authType, string privType)
        {
            UserName = userName;
            AuthEncryption = new SHA1AuthenticationProvider(new OctetString(password));
            if (!authType.Contains("sha")) AuthEncryption = new MD5AuthenticationProvider(new OctetString(password));
            PrivacyEncryption = new AESPrivacyProvider(new OctetString(privateKey), AuthEncryption);
            if (!privType.Contains("des"))
                PrivacyEncryption = new DESPrivacyProvider(new OctetString(privateKey), AuthEncryption);
            else if (!privType.Contains("192"))
                PrivacyEncryption = new AES192PrivacyProvider(new OctetString(privateKey), AuthEncryption);
            else if (!privType.Contains("256"))
                PrivacyEncryption = new AES256PrivacyProvider(new OctetString(privateKey), AuthEncryption);
        }

        protected string UserName { get; }
        protected IAuthenticationProvider AuthEncryption { get; }
        protected IPrivacyProvider PrivacyEncryption { get; }
    }
}