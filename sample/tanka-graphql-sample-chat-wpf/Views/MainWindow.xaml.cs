using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using System.Windows;
using Tanka.GraphQL.Sample.Chat.Client.Shared.Queries;

namespace Tanka.GraphQL.Sample.Chat.Client.Wpf.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

           // var _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            try
            {
                var connection = new HubConnectionBuilder()
                    .WithUrl("https://localhost:5001/hubs/graphql")
                    .Build();
                await connection.StartAsync();

                var channelsQuery = new ChannelsQuery(connection);
                var existingChannels = await channelsQuery.ExecuteAsync();

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
