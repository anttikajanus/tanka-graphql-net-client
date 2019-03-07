using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Tanka.GraphQL.Sample.Chat.Domain.Models;

namespace Tanka.GraphQL.Sample.Chat.Domain
{
    public class Chat
    {
        private readonly List<Channel> _channels = new List<Channel>();
        private int _nextChannelId = 1;
        private readonly List<Message> _messages = new List<Message>();
        private int _nextMessageId = 1;

        public Chat()
        {
            _messageAdded = new BroadcastBlock<Message>(message => message);
        }

        public Task<Channel> CreateChannelAsync(InputChannel inputChannel)
        {
            var channel = new Channel
            {
                Id = NextChannelId(),
                Name = inputChannel.Name
            };

            _channels.Add(channel);
            return Task.FromResult(channel);
        }

        public Task<IEnumerable<Channel>> GetChannelsAsync()
        {
            return Task.FromResult(_channels.AsEnumerable());
        }

        public Task<Channel> GetChannelAsync(int channelId)
        {
            var channel = _channels.SingleOrDefault(c => c.Id == channelId);
            return Task.FromResult(channel);
        }

        public Task<IEnumerable<Message>> GetMessagesAsync(int channelId, int latest = 100)
        {
            return Task.FromResult(_messages.AsEnumerable());
        }

        public async Task<Message> PostMessageAsync(int channelId, InputMessage inputMessage)
        {
            var channel = await GetChannelAsync(channelId);

            if (channel == null) throw new InvalidOperationException($"Channel '{channelId}' not found");

            var message = new Message
            {
                Id = NextMessageId(),
                Content = inputMessage.Content
            };
            _messages.Add(message);

            await _messageAdded.SendAsync(message);
            return message;
        }

        private readonly BroadcastBlock<Message> _messageAdded;
        public Task<IDisposable> JoinAsync(int channelId, BufferBlock<Message> target)
        {
            //todo: filter by channel
            var sub = _messageAdded.LinkTo(target, new DataflowLinkOptions()
            {
                PropagateCompletion = true
            });

            return Task.FromResult(sub);
        }

        private int NextChannelId()
        {
            return _nextChannelId++;
        }

        private int NextMessageId()
        {
            return _nextMessageId++;
        }
    }
}
