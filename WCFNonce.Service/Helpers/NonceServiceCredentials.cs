using System.IdentityModel.Selectors;
using System.ServiceModel.Description;

namespace WCFNonce.Service.Helpers
{
    public class NonceServiceCredentials : ServiceCredentials
    {
        public NonceServiceCredentials() : base()
        {
        }
        public NonceServiceCredentials(NonceServiceCredentials other) : base(other)
        {
        }

        protected override ServiceCredentials CloneCore()
        {
            return new NonceServiceCredentials(this);
        }

        public override SecurityTokenManager CreateSecurityTokenManager()
        {
            return new NonceServiceCredentialsSecurityTokenManager(this);
        }
    }
}