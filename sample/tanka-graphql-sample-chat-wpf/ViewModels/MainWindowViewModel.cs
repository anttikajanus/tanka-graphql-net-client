using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Tanka.GraphQL.Sample.Chat.Client.Shared;
using Tanka.GraphQL.Sample.Chat.Client.Shared.Services;
using Tanka.GraphQL.Sample.Chat.Client.Shared.ViewModels;
using Tanka.GraphQL.Sample.Chat.Client.Wpf.Services;

namespace Tanka.GraphQL.Sample.Chat.Client.Wpf.ViewModels
{
    public class MainWindowViewModel : BindableBase, IAsyncInitializer
    {
        private readonly IChatService _chatService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IDispatcherContext _dispatcherContext;
        private string _title = "Tanka Chat for WPF sample";
        private bool _isInitializing = false;

        private ObservableCollection<ChannelViewModel> _channels;
        private ChannelViewModel _selectedChannel;

        public MainWindowViewModel(IChatService chatService, IAuthenticationService authenticationService, IDispatcherContext dispatcherContext)
        {
            _chatService = chatService ?? throw new ArgumentNullException(nameof(chatService));
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
            _dispatcherContext = dispatcherContext ?? throw new ArgumentNullException(nameof(dispatcherContext));
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public bool IsInitializing
        {
            get { return _isInitializing; }
            set { SetProperty(ref _isInitializing, value); }
        }

        public ObservableCollection<ChannelViewModel> Channels
        {
            get { return _channels; }
            private set { SetProperty(ref _channels, value); }
        }

        public ChannelViewModel SelectedChannel
        {
            get { return _selectedChannel; }
            set { SetProperty(ref _selectedChannel, value); }
        }

        public async Task InitializeAsync(string serviceEndpoint)
        {
            try
            {
                IsInitializing = true;
                var loginResult = await _authenticationService.AuthenticateAsync();
                // Connect to the server
                await(_chatService as IAuthenticatedInitializer).InitializeAsync(serviceEndpoint, loginResult.AccessToken);

                // Query all available channels and create view models
                var channels = (await _chatService.GetAvailableChatChannelsAsync())
                    .Select(c => new ChannelViewModel(c, _chatService, _dispatcherContext));
                Channels = new ObservableCollection<ChannelViewModel>(channels);

                var tasks = Channels.Select(async channel => await channel.ConnectAsync());
                await Task.WhenAll(tasks);

                SelectedChannel = Channels.FirstOrDefault();
            }
            catch (HttpRequestException connectionException)
            {
                // Todo
                Debug.WriteLine(connectionException);
                throw;
            }
            catch (Exception ex)
            {
                // Todo
                Debug.WriteLine(ex);
                throw;
            }
            finally
            {
                IsInitializing = false;
            }
        }
    }
}
