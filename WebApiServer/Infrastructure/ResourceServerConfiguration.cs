using System.Security.Cryptography.X509Certificates;

namespace WebApiServer.Infrastructure
{
    public class ResourceServerConfiguration
    {
        public X509Certificate2 IssuerSigningCertificate { get; set; }
        public X509Certificate2 EncryptionVerificationCertificate { get; set; }
    }
}