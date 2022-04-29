namespace WCFNonce.Common
{
    public class NonceModel
    {
        private readonly string nonce = null;
        private readonly long timestamp = default;

        public NonceModel(string nonce, long timestamp)
        {
            this.nonce = nonce;
            this.timestamp = timestamp;
        }

        public string Nonce => 
            nonce;

        public long Timestamp =>
            timestamp;
    }
}