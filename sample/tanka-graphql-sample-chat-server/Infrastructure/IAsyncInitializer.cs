using System.Threading.Tasks;

namespace Tanka.GraphQL.Sample.Chat.Server.Infrastructure
{
    public interface IAsyncInitializer
    {
        Task InitializeAsync();
    }
}
