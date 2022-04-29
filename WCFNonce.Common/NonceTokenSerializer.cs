using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.ServiceModel.Security;
using System.Xml;

namespace WCFNonce.Common
{
    public class NonceTokenSerializer : WSSecurityTokenSerializer
    {
        public NonceTokenSerializer(SecurityTokenVersion version) : base() { }


        protected override bool CanReadTokenCore(XmlReader reader)
        {
            XmlDictionaryReader.CreateDictionaryReader(reader);

            if (reader == null) 
                throw new ArgumentNullException(nameof(reader));

            if (reader.IsStartElement(Constants.NonceTokenName, Constants.NonceTokenNamespace))
                return true;

            return base.CanReadTokenCore(reader);
        }

        protected override SecurityToken ReadTokenCore(XmlReader reader, SecurityTokenResolver tokenResolver)
        {

            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            if (reader.IsStartElement(Constants.NonceTokenName, Constants.NonceTokenNamespace))
            {
                string id = reader.GetAttribute(Constants.Id, Constants.WsUtilityNamespace);

                reader.ReadStartElement();

                string nonce = reader.ReadElementString(Constants.NonceElementName, Constants.NonceTokenNamespace);

                string timestampString = reader.ReadElementString(Constants.TimestampElementName, Constants.NonceTokenNamespace);
                long timestamp = XmlConvert.ToInt64(timestampString);

                reader.ReadEndElement();

                var nonceModel = new NonceModel(nonce, timestamp);

                return new NonceToken(nonceModel, id);
            }

            return WSSecurityTokenSerializer.DefaultInstance.ReadToken(reader, tokenResolver);
        }
        
        protected override bool CanWriteTokenCore(SecurityToken token)
        {
            if (token is NonceToken)
                return true;

            return base.CanWriteTokenCore(token);
        }

        protected override void WriteTokenCore(XmlWriter writer, SecurityToken token)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            if (token == null) 
                throw new ArgumentNullException(nameof(token));

            var nonceToken = token as NonceToken;
            if (nonceToken != null)
            {
                writer.WriteStartElement(Constants.NonceTokenPrefix, Constants.NonceTokenName, Constants.NonceTokenNamespace);
                
                writer.WriteAttributeString(Constants.WsUtilityPrefix, Constants.Id, Constants.WsUtilityNamespace, token.Id);
                writer.WriteElementString(Constants.NonceElementName, Constants.NonceTokenNamespace, nonceToken.NonceModel.Nonce);
                writer.WriteElementString(Constants.TimestampElementName, Constants.NonceTokenNamespace, nonceToken.NonceModel.Timestamp.ToString());
                
                writer.WriteEndElement();
                
                writer.Flush();
            }
            else
            {
                base.WriteTokenCore(writer, token);
            }

        }
    }
}
