using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;

namespace Tanka.GraphQL.Sample.Chat.Server.Infrastructure
{
    public static class AsyncInitializerExtensions
    {
        public static Task InitializeAsyncServices(this IWebHost host)
        {
            var asyncServices = host.Services.GetServices<IAsyncInitializer>();

            return Task.WhenAll(asyncServices.Select(service => service.InitializeAsync()));
        }
    }
}
