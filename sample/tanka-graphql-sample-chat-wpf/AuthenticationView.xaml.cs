using IdentityModel.OidcClient.Browser;
using Microsoft.Toolkit.Wpf.UI.Controls;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Tanka.GraphQL.Sample.Chat.Client.Wpf
{
    public partial class AuthenticationView : Window, IBrowser

    {
        public AuthenticationView()
        {
            InitializeComponent();
        }

        public async Task<BrowserResult> InvokeAsync(BrowserOptions options)
        {
            try
            {
                var grid = new Grid();

                MainContent.Content = grid;
                var browser = new WebView();

                var signal = new SemaphoreSlim(0, 1);

                var result = new BrowserResult
                {
                    ResultType = BrowserResultType.UserCancel
                };

                Closed += (o, e) =>
                {
                    signal.Release();
                };
                browser.DOMContentLoaded += (sender, args) =>
                {
                    Debug.WriteLine(args.Uri.ToString());
                    if (!args.Uri.ToString().Contains("blank"))
                    {
                        BusyIndication.Visibility = Visibility.Collapsed;
                    }
                    if (args.Uri.ToString().StartsWith(options.EndUrl))
                    {
                        result.ResultType = BrowserResultType.Success;

                        result.Response = args.Uri.ToString();

                        signal.Release();
                    }
                };

                grid.Children.Add(browser);
                Show();

                browser.Navigate(options.StartUrl);

                await signal.WaitAsync();
                grid.Children.Clear();

                return result;
            }
            finally
            {
                Close();
            }
        }
    }
}
