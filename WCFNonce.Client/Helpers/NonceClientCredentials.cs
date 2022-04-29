using System;
using System.IdentityModel.Selectors;
using System.ServiceModel.Description;
using WCFNonce.Common;

namespace WCFNonce.Client.Helpers
{
    public class NonceClientCredentials : ClientCredentials
    {
        NonceModel nonceModel;

        public NonceClientCredentials(NonceModel nonceModel)
            : base()
        {
            if (nonceModel == null)
                throw new ArgumentNullException(nameof(nonceModel));

            this.nonceModel = nonceModel;
        }

        public NonceModel NonceModel => 
            nonceModel;

        protected override ClientCredentials CloneCore()
        {
            return new NonceClientCredentials(this.nonceModel);
        }

        public override SecurityTokenManager CreateSecurityTokenManager()
        {
            return new NonceClientCredentialsSecurityTokenManager(this);
        }
    }
}