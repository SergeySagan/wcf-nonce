namespace WCFNonce.Common
{
    public static class Constants
    {
        public static string NonceTokenType => "https://github.com/SergeySagan/wcf-nonce/Tokens/NonceTokenType";

        public static string TimestampClaim => "https://github.com/SergeySagan/wcf-nonce/Claims/TimestampClaim";


        internal static string Id => "Id";

        internal static string NonceTokenPrefix => "nt";

        internal static string NonceTokenNamespace => "https://github.com/SergeySagan/wcf-nonce/Namespaces/NonceTokenNamespace";
        
        internal static string NonceTokenName => "NonceToken";

        internal static string NonceElementName => "Nonce";

        internal static string TimestampElementName => "Timestamp";

        internal static string WsUtilityPrefix => "wsu";

        internal static string WsUtilityNamespace => "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";
    }
}