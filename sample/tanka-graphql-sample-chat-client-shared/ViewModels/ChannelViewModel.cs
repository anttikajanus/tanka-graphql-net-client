using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tanka.GraphQL.Sample.Chat.Client.Shared.Models;
using Tanka.GraphQL.Sample.Chat.Client.Shared.Services;

namespace Tanka.GraphQL.Sample.Chat.Client.Shared.ViewModels
{
    public class ChannelViewModel : BindableBase
    {
        private readonly IChatService _chatService;
        private readonly IDispatcherContext _dispatcher;
        private readonly Channel _channel;
        private ObservableCollection<MessageViewModel> _messages;
        private readonly CancellationTokenSource _subscriptionSource;

        IDisposable _messagesSubscription = null;
        private string _newMessageContent;

        public ChannelViewModel(Channel channel, IChatService chatService, IDispatcherContext dispatcher)
        {
            _chatService = chatService ?? throw new ArgumentNullException(nameof(chatService));
            _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
            _channel = channel ?? throw new ArgumentNullException(nameof(channel));

            PostMessageCommand = new DelegateCommand<string>(PostMessage, CanPostMessage);
            CloseChannelCommand = new DelegateCommand(CloseChannel, CanCloseChannel);

            _subscriptionSource = new CancellationTokenSource();
        }

        public string Name => _channel.Name;

        public DelegateCommand<string> PostMessageCommand { get; }
        public DelegateCommand CloseChannelCommand { get; }

        public ObservableCollection<MessageViewModel> Messages
        {
            get { return _messages; }
            set { SetProperty(ref _messages, value); }
        }

        public string NewMessageContent
        {
            get { return _newMessageContent; }
            set { SetProperty(ref _newMessageContent, value); }
        }

        public async Task ConnectAsync()
        {
            var messages = (await _chatService.GetChannelMessagesAsync(_channel.Id))
                 .Select(message => new MessageViewModel(message));
            Messages = new ObservableCollection<MessageViewModel>(messages);

            var subscription = await _chatService.SubscribeToChannelMessagesAsync(_channel.Id, _subscriptionSource.Token);
            _messagesSubscription = subscription
                .Subscribe(x =>
                {
                    // Make sure that this is run in the UI thread. Could use ObserveOn instead
                    _dispatcher.Invoke(() =>
                    {
                        // Check if the message already exists
                        if (!Messages.Any(m => m.Id == x.Id))
                            Messages?.Add(new MessageViewModel(x));
                    });
                });
        }  

        private async void PostMessage(string messageContent)
        {
            try
            {
                if (!CanPostMessage(messageContent))
                    return; 

                var postedMessage = await _chatService.PostMessageAsync(messageContent, _channel.Id);
                NewMessageContent = string.Empty;
                if (!Messages.Any(m => m.Id == postedMessage.Id))
                    Messages.Add(new MessageViewModel(postedMessage));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private bool CanPostMessage(string messageContent)
        {
            return true;
        }

        private void CloseChannel()
        {
            if (!CanCloseChannel())
                return;

            _subscriptionSource.Cancel();
            _messagesSubscription.Dispose();
            CloseChannelCommand.RaiseCanExecuteChanged();
        }

        private bool CanCloseChannel()
        {
            return !_subscriptionSource.IsCancellationRequested;
        }
    }
}
