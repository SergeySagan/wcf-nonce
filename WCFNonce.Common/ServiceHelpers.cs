using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Claims;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security.Tokens;
using System.Text;

namespace WCFNonce.Common
{
    public static class ServiceHelpers
    {
        public static long StaleTimestampSeconds => 3;

        public static bool IsNonceValid()
        {
            var nonceModel = GetNonceModelFromRequest();

            if (string.IsNullOrWhiteSpace(nonceModel.Nonce))
                return false;

            if (nonceModel.Timestamp == default)
                return false;

            if (IsNonceStale(nonceModel.Timestamp))
                return false;

            return nonceModel.Nonce == CreateNonceModel(GetIpAddress(), nonceModel.Timestamp).Nonce;
        }

        public static NonceModel CreateNonceModel(string ipAddress, long? timestamp = null)
        {
            if (timestamp == null)
                timestamp = DateTime.UtcNow.Ticks;

            var privateKey = ConfigurationManager.AppSettings["NoncePrivateKey"];

            var decodedNonceBytes = Encoding.UTF8.GetBytes($"{ timestamp }:{ ipAddress }{ privateKey }");
            byte[] encodedNonce = null;
            using (var shaEncoder = new SHA256Managed())
            {
                encodedNonce = shaEncoder.ComputeHash(decodedNonceBytes);
            }

            if (encodedNonce == null)
                throw new Exception("Unknow error occurred. Failed to encode nonce.");

            return new NonceModel(Convert.ToBase64String(encodedNonce), timestamp.Value);
        }

        public static Binding CreateNonceBinding()
        {
            var httpTransport = new HttpTransportBindingElement();

            // The message security binding element will be configured to require a nonce.
            // The token that is encrypted with the service's certificate.
            var messageSecurity = new SymmetricSecurityBindingElement();

            messageSecurity.EndpointSupportingTokenParameters.SignedEncrypted.Add(new NonceTokenParameters());

            var x509ProtectionParameters = new X509SecurityTokenParameters();

            x509ProtectionParameters.InclusionMode = SecurityTokenInclusionMode.Never;

            messageSecurity.ProtectionTokenParameters = x509ProtectionParameters;

            return new CustomBinding(messageSecurity, httpTransport);
        }



        private static string GetIpAddress()
        {
            var context = OperationContext.Current;

            var messageProperties = context.IncomingMessageProperties;

            var endpoint = messageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;

            return endpoint.Address;
        }

        private static bool IsNonceStale(long timestamp)
        {
            var now = DateTime.UtcNow;

            if (timestamp < now.AddSeconds(-1 * StaleTimestampSeconds).Ticks)
                return true;

            if (timestamp > now.Ticks) // Hacker?
                return true;

            return false;
        }

        private static NonceModel GetNonceModelFromRequest()
        {
            string nonceValue = null;
            long timestampValue = default;

            foreach (ClaimSet claimSet in ServiceSecurityContext.Current.AuthorizationContext.ClaimSets)
            {
                if (TryGetStringClaimValue(claimSet.Issuer, ClaimTypes.Name, out string nonce))
                    nonceValue = nonce;

                if (TryGetStringClaimValue(claimSet, Constants.TimestampClaim, out string timestampString) &&
                        long.TryParse(timestampString, out long timestamp))
                    timestampValue = timestamp;
            }

            return new NonceModel(nonceValue, timestampValue);
        }

        private static bool TryGetStringClaimValue(ClaimSet claimSet, string claimType, out string claimValue)
        {
            claimValue = null;

            IEnumerable<Claim> matchingClaims = claimSet.FindClaims(claimType, Rights.PossessProperty);
            if (matchingClaims == null || !matchingClaims.Any())
                return false;

            IEnumerator<Claim> enumerator = matchingClaims.GetEnumerator();

            enumerator.MoveNext();

            claimValue = (enumerator.Current.Resource == null) ? 
                            null : enumerator.Current.Resource.ToString();

            return true;
        }
    }
}