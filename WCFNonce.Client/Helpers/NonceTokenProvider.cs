using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using WCFNonce.Common;

namespace WCFNonce.Client.Helpers
{
    public class NonceTokenProvider : SecurityTokenProvider
    {
        NonceModel nonceModel;

        public NonceTokenProvider(NonceModel nonceModel) : base()
        {
            if (nonceModel == null)
                throw new ArgumentNullException(nameof(nonceModel));

            this.nonceModel = nonceModel;
        }

        protected override SecurityToken GetTokenCore(TimeSpan timeout)
        {
            SecurityToken result = new NonceToken(nonceModel);

            return result;
        }
    }
}