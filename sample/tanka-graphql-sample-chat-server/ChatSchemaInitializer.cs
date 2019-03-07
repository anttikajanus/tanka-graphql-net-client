using System.Threading.Tasks;
using tanka.graphql.type;
using Tanka.GraphQL.Sample.Chat.Domain.Models;
using Tanka.GraphQL.Sample.Chat.Domain.Schemas;
using Tanka.GraphQL.Sample.Chat.Server.Infrastructure;
using static tanka.graphql.tools.SchemaTools;

namespace Tanka.GraphQL.Sample.Chat.Server
{
    public class ChatSchemaInitializer : IAsyncInitializer
    {
        public ISchema Schema { get; set; }

        public async Task InitializeAsync()
        {
            var idl = await SchemaLoader.LoadAsync();
          //  await idl.InitializeAsync();

            var chat = new Domain.Chat();

            // Add some test data
            await chat.CreateChannelAsync(new InputChannel()
            {
                Name = "Channel 1"
            });
            await chat.CreateChannelAsync(new InputChannel()
            {
                Name = "Channel 2"
            });

            var service = new ChatResolverService(chat);
            var resolvers = new ChatResolvers(service);

            Schema = await MakeExecutableSchemaWithIntrospection(
                idl,
                resolvers,
                resolvers);
        }
    }
}