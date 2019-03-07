using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tanka.GraphQL.Sample.Chat.Client.Shared.Models;

namespace Tanka.GraphQL.Sample.Chat.Client.Shared.Queries
{
    /// <summary>
    /// Query all channels from the service.
    /// </summary>
    public class ChannelsQuery : QueryBase<List<Channel>>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ChannelsQuery"/>
        /// </summary>
        /// <param name="connection">The used signalR connection.</param>
        public ChannelsQuery(HubConnection connection) : base(connection) { }

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <returns>Returns all the channels from the service.</returns>
        public async Task<List<Channel>> ExecuteAsync()
        {
            var channelsGQL = @"{channels {
                id
                name
              }
            }";

            var queryRequest = new QueryRequest()
            {
                Query = channelsGQL
            };

            return await ExecuteQueryAsync(queryRequest);
        }
    }
}
