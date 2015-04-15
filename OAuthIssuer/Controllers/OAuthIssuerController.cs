using System.Web.Mvc;
using DotNetOpenAuth.OAuth2;
using System.Security.Cryptography.X509Certificates;
using DotNetOpenAuth.Messaging;
using OAuthIssuer.Infrastructure;

namespace OAuthIssuer.Controllers
{
public class OAuthIssuerController : Controller
{
    public ActionResult Index()
    {
        var configuration = new IssuerConfiguration
        {
            EncryptionCertificate = new X509Certificate2(Server.MapPath("~/Certs/localhost.cer")),
            SigningCertificate = new X509Certificate2(Server.MapPath("~/Certs/localhost.pfx"), "a")
        };

        var authorizationServer = new AuthorizationServer(new OAuth2Issuer(configuration));
        var response = authorizationServer.HandleTokenRequest(Request).AsActionResult();

        return response;
    }
}
}
