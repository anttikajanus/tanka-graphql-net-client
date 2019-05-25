using IdentityModel.OidcClient;
using System.Threading.Tasks;

namespace Tanka.GraphQL.Sample.Chat.Client.Wpf.Services
{
    public interface IAuthenticationService
    {
        Task<LoginResult> AuthenticateAsync();
        Task LogoutAsync();
    }
}