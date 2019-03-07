using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tanka.GraphQL.Sample.Chat.Client.Shared.Models;

namespace Tanka.GraphQL.Sample.Chat.Client.Shared.Services
{
    public interface IChatService
    {
        Task<List<Channel>> GetAvailableChatChannelsAsync();
        Task<List<Message>> GetChannelMessagesAsync(int channelId);
        Task<Message> PostMessageAsync(string messageContent, int channelId);
        Task<IObservable<Message>> SubscribeToChannelMessagesAsync(int channelId, CancellationToken token);
    }
}