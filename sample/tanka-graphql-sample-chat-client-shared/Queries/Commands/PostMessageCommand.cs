using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tanka.GraphQL.Sample.Chat.Client.Shared.Models;

namespace Tanka.GraphQL.Sample.Chat.Client.Shared.Queries.Commands
{
    /// <summary>
    /// Command to send new message to the target channel.
    /// </summary>
    public class PostMessageCommand : QueryBase<Message>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="PostMessageCommand"/>
        /// </summary>
        /// <param name="connection">The used signalR connection.</param>
        public PostMessageCommand(HubConnection connection) : base(connection) { }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="channelId">The id of the target channel.</param>
        /// <param name="message">The content of the new message.</param>
        public async Task<Message> ExecuteAsync(int channelId, string message)
        {
            var postMessageMutationGQL =
                @"mutation PostMessage($channelId: Int!, $message: InputMessage!) {
                    postMessage(channelId: $channelId, message: $message) {
                     id
                     channelId
                     content
                     timestamp
                     from
                     profileUrl
                  }
                }";

            var queryRequest = new QueryRequest()
            {
                Query = postMessageMutationGQL,
                Variables = new Dictionary<string, object>()
                {
                    { "channelId", channelId },
                    { "message", new InputMessage() { Content = message } }
                }
            };

            return await ExecuteQueryAsync(queryRequest);
        }
    }
}
