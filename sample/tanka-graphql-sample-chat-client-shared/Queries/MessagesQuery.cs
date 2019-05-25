using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tanka.GraphQL.Sample.Chat.Client.Shared.Models;

namespace Tanka.GraphQL.Sample.Chat.Client.Shared.Queries
{
    /// <summary>
    /// Query all messages from the target channel.
    /// </summary>
    public class MessagesQuery : QueryBase<List<Message>>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="MessagesQuery"/>
        /// </summary>
        /// <param name="connection">The used signalR connection.</param>
        public MessagesQuery(HubConnection connection) : base(connection) { }

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <param name="channelId">The id of the target channel.</param>
        /// <returns>Returns all the messges from the channel.</returns>
        public async Task<List<Message>> ExecuteAsync(int channelId)
        {
            var channelMessageGQL =
                @"query Messages($channelId: Int!) {
                    messages(channelId: $channelId) {
                      id
                      channelId
                      content
                      timestamp
                      from
                    }
                }";

            var queryRequest = new QueryRequest()
            {
                Query = channelMessageGQL,
                Variables = new Dictionary<string, object>()
                {
                    { "channelId", channelId}
                }
            };

            return await ExecuteQueryAsync(queryRequest);
        }
    }
}
