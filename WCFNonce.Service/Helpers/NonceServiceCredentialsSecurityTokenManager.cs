using System.IdentityModel.Selectors;
using System.ServiceModel.Security;
using WCFNonce.Common;

namespace WCFNonce.Service.Helpers
{
    public class NonceServiceCredentialsSecurityTokenManager : ServiceCredentialsSecurityTokenManager
    {
        NonceServiceCredentials nonceServiceCredentials;

        public NonceServiceCredentialsSecurityTokenManager(NonceServiceCredentials nonceServiceCredentials)
            : base(nonceServiceCredentials)
        {
            this.nonceServiceCredentials = nonceServiceCredentials;
        }

        public override SecurityTokenAuthenticator CreateSecurityTokenAuthenticator(SecurityTokenRequirement tokenRequirement, 
                                                                                    out SecurityTokenResolver outOfBandTokenResolver)
        {
            if (tokenRequirement.TokenType == Constants.NonceTokenType)
            {
                outOfBandTokenResolver = null;

                return new NonceTokenAuthenticator();
            }

            return base.CreateSecurityTokenAuthenticator(tokenRequirement, out outOfBandTokenResolver);
        }

        public override SecurityTokenSerializer CreateSecurityTokenSerializer(SecurityTokenVersion version)
        {
            return new NonceTokenSerializer(version);
        }
    }
}