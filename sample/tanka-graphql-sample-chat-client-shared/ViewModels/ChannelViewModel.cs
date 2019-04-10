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
        private IDisposable _messagesSubscription = null;
        private ObservableCollection<MessageViewModel> _messages;
        private CancellationTokenSource _subscriptionSource;
        private string _newMessageContent;

        public ChannelViewModel(Channel channel, IChatService chatService, IDispatcherContext dispatcher)
        {
            _chatService = chatService ?? throw new ArgumentNullException(nameof(chatService));
            _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
            _channel = channel ?? throw new ArgumentNullException(nameof(channel));

            PostMessageCommand = new DelegateCommand<string>(PostMessage, CanPostMessage);
            CloseChannelCommand = new DelegateCommand(CloseChannel, CanCloseChannel);
            ConnectChannelCommand = new DelegateCommand(ConnectChannel, CanConnectChannel);
        }

        public string Name => _channel.Name;

        public DelegateCommand<string> PostMessageCommand { get; }

        public DelegateCommand CloseChannelCommand { get; }

        public DelegateCommand ConnectChannelCommand { get; }

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
            _subscriptionSource = new CancellationTokenSource();
            var messages = (await _chatService.GetChannelMessagesAsync(_channel.Id))
                .Select(message => new MessageViewModel(message));
            if (Messages == null)
                Messages = new ObservableCollection<MessageViewModel>(messages);
            else
                foreach (var message in messages.Where(m => !Messages.Any(x => x.Id == m.Id)))
                    Messages.Add(message);

            var subscription = await _chatService.SubscribeToChannelMessagesAsync(_channel.Id, _subscriptionSource.Token);
            _messagesSubscription = subscription
                .Subscribe(x =>
                {
                    // Make sure that this is run in the UI thread. Could use ObserveOn instead
                    _dispatcher.Invoke(() =>
                    {
                        if (!Messages.Any(m => m.Id == x.Id))
                            Messages?.Add(new MessageViewModel(x));
                    });
                });
        }

        private async void PostMessage(string messageContent)
        {
            if (!CanPostMessage(messageContent))
                return;

            var postedMessage = await _chatService.PostMessageAsync(messageContent, _channel.Id);
            NewMessageContent = string.Empty;
            if (!Messages.Any(m => m.Id == postedMessage.Id))
                Messages.Add(new MessageViewModel(postedMessage));
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
            ConnectChannelCommand.RaiseCanExecuteChanged();
        }

        private bool CanCloseChannel()
        {
            return !_subscriptionSource.IsCancellationRequested && _messagesSubscription != null;
        }

        private async void ConnectChannel()
        {
            if (!CanConnectChannel())
                return;

            await ConnectAsync();
            CloseChannelCommand.RaiseCanExecuteChanged();
            ConnectChannelCommand.RaiseCanExecuteChanged();
        }

        private bool CanConnectChannel()
        {
            if (_subscriptionSource == null || _subscriptionSource.IsCancellationRequested)
                return true;
            return false;
        }
    }
}
