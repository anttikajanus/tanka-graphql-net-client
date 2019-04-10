using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tanka.GraphQL.Sample.Chat.Client.Shared;
using Tanka.GraphQL.Sample.Chat.Client.Shared.Services;
using Cons = System.Console;

namespace Tanka.GraphQL.Sample.Chat.Client.Console
{
    public class Program
    {
        private static ChatService _chatService;
        private static List<IDisposable> _channelSubscriptions;

        public static async Task Main(string[] args)
        {
            _channelSubscriptions = new List<IDisposable>();
            _chatService = new ChatService();

            Cons.WriteLine("Welcome to the Tanka chat application log.");
            Cons.WriteLine("This will log each submitted message to the log that is sent to the server from chat applications.");
            Cons.WriteLine("Press any key to close the console.");
            Cons.WriteLine("--------------------------------------------------------------------------------------------");
            Cons.WriteLine();
            await ConnectChatAsync();
            Cons.ReadKey();
            Cons.WriteLine("");
            Cons.WriteLine("Shutting down...");
            await DisconnectChatAsync();
        }

        private static async Task ConnectChatAsync()
        {
            try
            {
                // Connect to the server
                await (_chatService as IAsyncInitializer).InitializeAsync("https://tanka-chat.azurewebsites.net/hubs/graphql");

                // Get all channels and log them out
                var channels = await _chatService.GetAvailableChatChannelsAsync();
                Cons.WriteLine("Currently existing channels:");
                channels.ForEach(c => Cons.WriteLine($"Channel [{c.Id}] : {c.Name}"));
                Cons.WriteLine("");
                Cons.WriteLine("Messages log:");
                // Subscribe to each channel
                foreach (var channel in channels)
                {
                    var subscribtion = await _chatService.SubscribeToChannelMessagesAsync(channel.Id, default(CancellationToken));
                    var channelSubscription = subscribtion.Subscribe(message =>
                    {
                        Cons.WriteLine($"[{DateTime.Now.ToLongTimeString()}] {channel.Name} : {message.Content}");
                    });
                    _channelSubscriptions.Add(channelSubscription);
                }
            }
            catch (Exception ex)
            {
                Cons.WriteLine(ex);
            }
        }

        private static async Task DisconnectChatAsync()
        {
            try
            {
                foreach (var subscription in _channelSubscriptions)
                {
                    subscription.Dispose();
                }
            }
            catch (Exception ex)
            {
                Cons.Write(ex.ToString());
                Cons.ReadKey();
            }
        }
    }
}
