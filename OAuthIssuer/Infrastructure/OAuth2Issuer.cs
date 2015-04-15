using System;
using System.Security.Cryptography;
using DotNetOpenAuth.OAuth2;
using OAuthIssuer.Controllers;

namespace OAuthIssuer.Infrastructure
{
    public class OAuth2Issuer : IAuthorizationServer
    {
        private readonly IssuerConfiguration _configuration;

        public OAuth2Issuer(IssuerConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            _configuration = configuration;
        }

        public RSACryptoServiceProvider AccessTokenSigningKey
        {
            get
            {
                return (RSACryptoServiceProvider)_configuration.SigningCertificate.PrivateKey;
            }
        }

        public DotNetOpenAuth.Messaging.Bindings.ICryptoKeyStore CryptoKeyStore
        {
            get { throw new NotImplementedException(); }
        }

        public TimeSpan GetAccessTokenLifetime(DotNetOpenAuth.OAuth2.Messages.IAccessTokenRequest accessTokenRequestMessage)
        {
            return _configuration.TokenLifetime;
        }

        public IClientDescription GetClient(string clientIdentifier)
        {
            const string secretPassword = "test1243";
            return new ClientDescription(secretPassword, new Uri("http://localhost/"), ClientType.Confidential);
        }

        public RSACryptoServiceProvider GetResourceServerEncryptionKey(DotNetOpenAuth.OAuth2.Messages.IAccessTokenRequest accessTokenRequestMessage)
        {
            return (RSACryptoServiceProvider)_configuration.EncryptionCertificate.PublicKey.Key;

        }

        public bool IsAuthorizationValid(DotNetOpenAuth.OAuth2.ChannelElements.IAuthorizationDescription authorization)
        {
            
            //claims added to the token
            authorization.Scope.Add("adminstrator");
            authorization.Scope.Add("poweruser");
            
            return true;
        }

        public bool IsResourceOwnerCredentialValid(string userName, string password)
        {
            return true;
        }

        public DotNetOpenAuth.Messaging.Bindings.INonceStore VerificationCodeNonceStore
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}