using System.Threading.Tasks;

namespace Tanka.GraphQL.Sample.Chat.Client.Shared.Services
{
    public interface IAsyncInitializer
    {
        Task InitializeAsync(string serviceEndpoint);
    }
}