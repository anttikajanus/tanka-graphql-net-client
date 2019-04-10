using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tanka.GraphQL.Sample.Chat.Client.Shared.Models;

namespace Tanka.GraphQL.Sample.Chat.Client.Shared.Queries.Subscriptions
{
    /// <summary>
    /// Query to subscripe to the new message added on a channel.
    /// </summary>
    public class MessageAddedSubscription : SubscriptionBase<Message>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="MessageAddedSubscription"/>
        /// </summary>
        /// <param name="connection">The used signalR connection.</param>
        public MessageAddedSubscription(HubConnection connection) : base(connection) { }

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <param name="channelId">The id of the target channel.</param>
        /// <returns>Returns <see cref="IObservable{T}"/> that pushes notificatons to the subscribers when new messages were added.</returns>
        public async Task<IObservable<Message>> ExecuteAsync(int channelId, CancellationToken token)
        {
            var channelMessagesSubsriptionGQL = 
                @"subscription MessageAdded($channelId: Int!) {
                    messageAdded(channelId: $channelId) {
                      id
                      content
                    }
                }";

            var queryRequest = new QueryRequest()
            {
                Query = channelMessagesSubsriptionGQL,
                Variables = new Dictionary<string, object>()
                {
                    { "channelId", channelId}
                }
            };

            var subscription = await SubscribeAsync(queryRequest, token);
            return subscription;
        }
    }
}
