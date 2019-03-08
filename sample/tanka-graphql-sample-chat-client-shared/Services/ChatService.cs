using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
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
    public class ChatService : IChatService, IAsyncInitializer
    {
        private HubConnection _connection;   
        private ChannelsQuery _channelsQuery;
        private MessagesQuery _messagesQuery;
        private PostMessageCommand _postMessageCommand;
        private MessageAddedSubscription _messageAddedSubscription;

        public async Task<List<Channel>> GetAvailableChatChannelsAsync()
        {
            var result = await _channelsQuery.ExecuteAsync();
            return result;
        }

        public async Task<List<Message>> GetChannelMessagesAsync(int channelId)
        {
            var result = await _messagesQuery.ExecuteAsync(channelId);
            return result;
        }

        public async Task<Message> PostMessageAsync(string messageContent, int channelId)
        {
            var result = await _postMessageCommand.ExecuteAsync(channelId, messageContent);
            return result;
        }

        public async Task<IObservable<Message>> SubscribeToChannelMessagesAsync(int channelId, CancellationToken token)
        {
            var result = await _messageAddedSubscription.ExecuteAsync(channelId, token);
            return result;
        }

        public async Task InitializeAsync(string serviceEndpoint)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(serviceEndpoint)
                .Build();
            await _connection.StartAsync();

            _channelsQuery = new ChannelsQuery(_connection);
            _messagesQuery = new MessagesQuery(_connection);
            _postMessageCommand = new PostMessageCommand(_connection);
            _messageAddedSubscription = new MessageAddedSubscription(_connection);
        }
    }
}
