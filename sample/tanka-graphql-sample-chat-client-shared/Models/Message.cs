using System;

namespace Tanka.GraphQL.Sample.Chat.Client.Shared.Models
{
    public class Message
    {
        public Message()
        {
            Timestamp = DateTimeOffset.UtcNow;
        }

        public DateTimeOffset? Timestamp { get; set; }

        public int Id { get; set; }

        public string Content { get; set; }

        public int ChannelId { get; set; }

        public string From { get; set; }
    }
}
