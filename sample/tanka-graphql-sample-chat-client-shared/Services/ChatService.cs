using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Tanka.GraphQL.Sample.Chat.Client.Shared.Models;
using Tanka.GraphQL.Sample.Chat.Client.Shared.Queries;
using Tanka.GraphQL.Sample.Chat.Client.Shared.Queries.Commands;
using Tanka.GraphQL.Sample.Chat.Client.Shared.Queries.Subscriptions;

namespace Tanka.GraphQL.Sample.Chat.Client.Shared.Services
{
    /// <summary>
    /// Provides chat functionality in one place.
    /// </summary>
    public class ChatService : IChatService, IAuthenticatedInitializer
    {
        private HubConnection _connection;   

        public async Task<List<Channel>> GetAvailableChatChannelsAsync()
        {
            var channels = await Task.Run(async () =>
            {
                var channelsQuery = new ChannelsQuery(_connection);
                var result = await channelsQuery.ExecuteAsync();
                return result;
            });
            return channels;
        }

        public async Task<List<Message>> GetChannelMessagesAsync(int channelId)
        {
            var messages = await Task.Run(async() => 
            {
                var messageQuery = new MessagesQuery(_connection);
                var result = await messageQuery.ExecuteAsync(channelId);
                return result;
            });
            return messages;
        }

        public async Task<Message> PostMessageAsync(string messageContent, int channelId)
        {
            var message = await Task.Run(async () =>
            {
                var postMessagesCommand = new PostMessageCommand(_connection);
                var result = await postMessagesCommand.ExecuteAsync(channelId, messageContent);
                return result;
            });
            return message;
        }

        public async Task<IObservable<Message>> SubscribeToChannelMessagesAsync(int channelId, CancellationToken token)
        {
            var subscription = await Task.Run(async () => 
            {
                var messageAddedSubscription = new MessageAddedSubscription(_connection);
                var result = await messageAddedSubscription.ExecuteAsync(channelId, token);
                return result;
            });
            return subscription;
        }

        public async Task InitializeAsync(string serviceEndpoint, string identityToken)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl( 
                    serviceEndpoint, options => {
                            options.AccessTokenProvider = async () => {
                                return identityToken;
                            };
                    }).
                Build();
            await _connection.StartAsync();
        }
    }
}
