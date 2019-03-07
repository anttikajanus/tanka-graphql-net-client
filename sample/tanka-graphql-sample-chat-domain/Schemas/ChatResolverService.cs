using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using tanka.graphql.resolvers;
using Tanka.GraphQL.Sample.Chat.Domain.Models;
using static tanka.graphql.resolvers.Resolve;

namespace Tanka.GraphQL.Sample.Chat.Domain.Schemas
{
    public class ChatResolverService
    {
        private readonly Chat _chat;

        public ChatResolverService(Chat chat)
        {
            _chat = chat;
        }

        public async Task<IResolveResult> Channels(ResolverContext context)
        {
            var channels = await _chat.GetChannelsAsync();
            return As(channels);
        }

        public async Task<IResolveResult> ChannelMessages(ResolverContext context)
        {
            var channelId = (int)(long)context.Arguments["channelId"];

            var messages = await _chat.GetMessagesAsync(channelId);
            return As(messages);
        }

        //public Task<IResolveResult> ChannelMembers(ResolverContext context)
        //{
        //    var member = new Member
        //    {
        //        Id = 1,
        //        Name = "Fugu"
        //    };

        //    return Task.FromResult(As(new[] {member}));
        //}

        public async Task<IResolveResult> PostMessage(ResolverContext context)
        {
            var channelId = (int)(long)context.Arguments["channelId"];
            var inputMessage = context.GetArgument<InputMessage>("message");
            var message = await _chat.PostMessageAsync(channelId, inputMessage);
            return As(message);
        }

        //public async Task<IResolveResult> Channel(ResolverContext context)
        //{
        //    var channelId = (int) (long) context.Arguments["channelId"];
        //    var channel = await _chat.GetChannelAsync(channelId);

        //    return As(channel);
        //}

        public async Task<ISubscribeResult> SubscribeToChannel(ResolverContext context, CancellationToken unsubscribe)
        {
            var channelId = (int)(long)context.Arguments["channelId"];
            var target = new BufferBlock<Message>();
            var subscription = await _chat.JoinAsync(channelId, target);
            unsubscribe.Register(() =>
            {
                subscription.Dispose();
                target.Complete();
            });

            return Stream(target);
        }

        public Task<IResolveResult> Message(ResolverContext context)
        {
            return Task.FromResult(As(context.ObjectValue));
        }
    }
}
