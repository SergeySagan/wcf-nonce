using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.ServiceModel.Security.Tokens;

namespace WCFNonce.Common
{
    public class NonceTokenParameters : SecurityTokenParameters
    {
        public NonceTokenParameters()
        { 
        }

        protected NonceTokenParameters(NonceTokenParameters other)
            : base(other)
        {
        }

        protected override SecurityTokenParameters CloneCore()
        {
            return new NonceTokenParameters(this);
        }

        protected override void InitializeSecurityTokenRequirement(SecurityTokenRequirement requirement)
        {
            requirement.TokenType = Constants.NonceTokenType;

            return;
        }

        // A nonce token has no crypto, no windows identity and supports only client authentication
        protected override bool HasAsymmetricKey { get { return false; } }

        protected override bool SupportsClientAuthentication { get { return true; } }
        
        protected override bool SupportsClientWindowsIdentity { get { return false; } }
        
        protected override bool SupportsServerAuthentication { get { return false; } }

        protected override SecurityKeyIdentifierClause CreateKeyIdentifierClause(SecurityToken token, 
                                                                                 SecurityTokenReferenceStyle referenceStyle)
        {
            if (referenceStyle == SecurityTokenReferenceStyle.Internal)
                return token.CreateKeyIdentifierClause<LocalIdKeyIdentifierClause>();

            throw new NotSupportedException("External references are not supported for nonce tokens");
        }
    }
}