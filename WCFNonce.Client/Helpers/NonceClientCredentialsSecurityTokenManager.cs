using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.ServiceModel;
using System.ServiceModel.Security.Tokens;
using WCFNonce.Common;

namespace WCFNonce.Client.Helpers
{
    public class NonceClientCredentialsSecurityTokenManager : ClientCredentialsSecurityTokenManager
    {
        NonceClientCredentials nonceClientCredentials;

        public NonceClientCredentialsSecurityTokenManager(NonceClientCredentials nonceClientCredentials)
            : base(nonceClientCredentials)
        {
            this.nonceClientCredentials = nonceClientCredentials;
        }

        public override SecurityTokenProvider CreateSecurityTokenProvider(SecurityTokenRequirement tokenRequirement)
        {
            // handle this token for Custom
            if (tokenRequirement.TokenType == Constants.NonceTokenType)
                return new NonceTokenProvider(this.nonceClientCredentials.NonceModel);

            // return server cert
            else if (tokenRequirement is InitiatorServiceModelSecurityTokenRequirement)
            {
                if (tokenRequirement.TokenType == SecurityTokenTypes.X509Certificate)
                    return new X509SecurityTokenProvider(nonceClientCredentials.ServiceCertificate.DefaultCertificate);
            }

            return base.CreateSecurityTokenProvider(tokenRequirement);
        }

        public override SecurityTokenSerializer CreateSecurityTokenSerializer(SecurityTokenVersion version)
        {
            return new NonceTokenSerializer(version);
        }
    }
}