using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using DotNetOpenAuth.OAuth2;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter to run demo.");
            Console.ReadLine();
            var state = GetAccessToken();

            Console.WriteLine("Expires = {0}", state.AccessTokenExpirationUtc.Value.ToLocalTime());
            Console.WriteLine("Token = {0}", state.AccessToken);

            var httpClient = new OAuthHttpClient(state.AccessToken)
            {
                BaseAddress = new Uri("http://localhost:2150/api/values")
            };

            Console.WriteLine("Calling web api...");

            Console.WriteLine();

            var response = httpClient.GetAsync("").Result;
            
            if (response.StatusCode==HttpStatusCode.OK)
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            else
                Console.WriteLine(response);

            Console.ReadLine();
        }
 



        private static IAuthorizationState GetAccessToken()
        {

            var authorizationServer = new AuthorizationServerDescription
            {
                TokenEndpoint = new Uri("http://localhost:1961/Issuer"),
                ProtocolVersion = ProtocolVersion.V20

            };
            var client = new WebServerClient(authorizationServer, "http://localhost/");
            client.ClientIdentifier = "zamd";
            client.ClientSecret = "test1243";
         
            var state = client.GetClientAccessToken(new[] { "http://localhost/" });
            return state;
        }
    }

    public class OAuthHttpClient : HttpClient
    {
        public OAuthHttpClient(string accessToken)
            : base(new OAuthTokenHandler(accessToken))
        {

        }

        class OAuthTokenHandler : MessageProcessingHandler
        {
            string _accessToken;
            public OAuthTokenHandler(string accessToken)
                : base(new HttpClientHandler())
            {
                _accessToken = accessToken;

            }
            protected override HttpRequestMessage ProcessRequest(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
                return request;
            }

            protected override HttpResponseMessage ProcessResponse(HttpResponseMessage response, System.Threading.CancellationToken cancellationToken)
            {
                return response;
            }
        }

    }
}
