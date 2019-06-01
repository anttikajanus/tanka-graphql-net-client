using Auth0.OidcClient;
using IdentityModel.OidcClient;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Tanka.GraphQL.Sample.Chat.Client.Wpf.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private Auth0Client _client = null;

        public async Task<LoginResult> AuthenticateAsync()
        {
            string domain = ConfigurationManager.AppSettings["Auth0:Domain"];
            string clientId = ConfigurationManager.AppSettings["Auth0:ClientId"];
            string audience = ConfigurationManager.AppSettings["Auth0:Audience"];
            string connection = ConfigurationManager.AppSettings["Auth0:Connection"];
            string scope = ConfigurationManager.AppSettings["Auth0:Scope"];
          
            _client = new Auth0Client(new Auth0ClientOptions
            {
                Domain = domain,
                ClientId = clientId,
                Browser = new EdgeWebView("Tanka Chat for WPF sample", 600, 810),
                Scope = scope
            });

            var extraParameters = new Dictionary<string, string>()
            {
                { "connection", connection },
                { "audience", audience }
            };
            var loginResult = await _client.LoginAsync(extraParameters: extraParameters);
            if (loginResult.IsError)
            {
                Debug.WriteLine(loginResult.Error);
            }

            return loginResult;
        }

        public Task LogoutAsync()
        {
            return _client.LogoutAsync();
        }
    }
}
