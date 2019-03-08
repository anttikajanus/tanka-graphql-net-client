using System.Threading.Tasks;

namespace Tanka.GraphQL.Sample.Chat.Client.Shared
{
    public interface IAsyncInitializer
    {
        Task InitializeAsync(string serviceEndpoint);
    }
}