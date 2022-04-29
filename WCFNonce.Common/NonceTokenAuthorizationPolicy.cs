using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;

namespace WCFNonce.Common
{
    public class NonceTokenAuthorizationPolicy : IAuthorizationPolicy
    {
        string id;
        ClaimSet issuer;
        IEnumerable<ClaimSet> issuedClaimSets;

        public NonceTokenAuthorizationPolicy(ClaimSet issuedClaims)
        {
            if (issuedClaims == null)
                throw new ArgumentNullException(nameof(issuedClaims));

            this.issuer = issuedClaims.Issuer;
            this.issuedClaimSets = new ClaimSet[] { issuedClaims };
            this.id = Guid.NewGuid().ToString();
        }

        public ClaimSet Issuer { get { return this.issuer; } }

        public string Id { get { return this.id; } }



        public bool Evaluate(EvaluationContext context, ref object state)
        {
            foreach (ClaimSet issuance in this.issuedClaimSets)
            {
                context.AddClaimSet(this, issuance);
            }

            return true;
        }
    }
}