using tanka.graphql;
using Tanka.GraphQL.Sample.Chat.Domain.Models;
using static tanka.graphql.resolvers.Resolve;

namespace Tanka.GraphQL.Sample.Chat.Domain.Schemas
{
    public class ChatResolvers : ResolverMap
    {
        public ChatResolvers(ChatResolverService resolver)
        {
            // Root resolver mapping
            this["Query"] = new FieldResolverMap
            {
                {"channels", resolver.Channels},
                //{"channel", resolver.Channel},
                {"messages", resolver.ChannelMessages}
            };

            this["Mutation"] = new FieldResolverMap()
            {
                {"postMessage", resolver.PostMessage}
            };

            this["Subscription"] = new FieldResolverMap()
            {
                {"messageAdded", resolver.SubscribeToChannel, resolver.Message}
            };

            // Domain mapping
            this["Channel"] = new FieldResolverMap
            {
                {"id", PropertyOf<Channel>(c => c.Id)},
                {"name", PropertyOf<Channel>(c => c.Name)}
            };

            this["Message"] = new FieldResolverMap
            {
                {"id", PropertyOf<Message>(m => m.Id)},
                {"content", PropertyOf<Message>(m => m.Content)}
            };
        }
    }
}