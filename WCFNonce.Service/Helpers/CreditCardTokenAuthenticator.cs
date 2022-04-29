using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using WCFNonce.Common;

namespace WCFNonce.Service.Helpers
{
    public class NonceTokenAuthenticator : SecurityTokenAuthenticator
    {
        protected override bool CanValidateTokenCore(SecurityToken token)
        {
            return (token is NonceToken);
        }

        protected override ReadOnlyCollection<IAuthorizationPolicy> ValidateTokenCore(SecurityToken token)
        {
            var nonceToken = token as NonceToken;

            if (string.IsNullOrWhiteSpace(nonceToken.NonceModel.Nonce))
                throw new SecurityTokenValidationException("The Nonce is required.");

            if (nonceToken.NonceModel.Timestamp == default)
                throw new SecurityTokenValidationException("The Timestamp is required.");

            var nonceClaimSet = new DefaultClaimSet(new Claim(ClaimTypes.Name, nonceToken.NonceModel.Nonce, Rights.PossessProperty));
            var timestampClaimSet = new DefaultClaimSet(nonceClaimSet, new Claim(Constants.TimestampClaim, nonceToken.NonceModel.Timestamp, Rights.PossessProperty));

            var policies = new List<IAuthorizationPolicy>(1);

            policies.Add(new NonceTokenAuthorizationPolicy(timestampClaimSet));
            
            return policies.AsReadOnly();
        }
    }
}