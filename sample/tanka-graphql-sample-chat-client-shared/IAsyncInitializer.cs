using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Tanka.GraphQL.Sample.Chat.Client.Shared
{
    public interface IAsyncInitializer
    {
        Task InitializeAsync(string serviceEndpoint);
    }

    public interface IAuthenticatedInitializer
    {
        Task InitializeAsync(string serviceEndpoint, string identityToken);
    }

}