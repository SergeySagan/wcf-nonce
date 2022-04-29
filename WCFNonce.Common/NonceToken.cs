using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;

namespace WCFNonce.Common
{
    public class NonceToken : SecurityToken
    {
        NonceModel nonceModel;
        DateTime effectiveTime = DateTime.UtcNow;
        string id;
        ReadOnlyCollection<SecurityKey> securityKeys;

        public NonceToken(NonceModel nonceModel) : this(nonceModel, Guid.NewGuid().ToString()) { }

        public NonceToken(NonceModel nonceModel, string id)
        {
            if (nonceModel == null)
                throw new ArgumentNullException(nameof(nonceModel));

            if (id == null)
                throw new ArgumentNullException(nameof(id));

            this.nonceModel = nonceModel;
            this.id = id;

            // the token is not capable of any crypto
            this.securityKeys = new ReadOnlyCollection<SecurityKey>(new List<SecurityKey>());
        }

        public NonceModel NonceModel { get { return this.nonceModel; } }

        public override ReadOnlyCollection<SecurityKey> SecurityKeys { get { return this.securityKeys; } }

        public override DateTime ValidFrom { get { return this.effectiveTime; } }

        public override DateTime ValidTo { get { return this.effectiveTime.AddSeconds(ServiceHelpers.StaleTimestampSeconds); } }

        public override string Id { get { return this.id; } }
    }
}
