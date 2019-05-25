using IdentityModel.OidcClient.Browser;
using Microsoft.Toolkit.Wpf.UI.Controls;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Tanka.GraphQL.Sample.Chat.Client.Wpf
{
    public class EdgeWebView : IBrowser
    {
        private readonly Func<Window> _windowFactory;
        private readonly bool _shouldCloseWindow;

       
        public EdgeWebView(Func<Window> windowFactory, bool shouldCloseWindow = true)
        {
            _windowFactory = windowFactory;
            _shouldCloseWindow = shouldCloseWindow;
        }

        public EdgeWebView(string title = "Authenticating ...", int width = 1024, int height = 768)
            : this(() => new Window
            {
                Name = "WebAuthentication",
                Title = title,
                Width = width,
                Height = height
            })
        {
            _shouldCloseWindow = true;
        }

        public async Task<BrowserResult> InvokeAsync(BrowserOptions options)
        {
            var window = _windowFactory.Invoke();

            try
            {
                var grid = new Grid();

                window.Content = grid;
                var browser = new WebView();

                var signal = new SemaphoreSlim(0, 1);

                var result = new BrowserResult
                {
                    ResultType = BrowserResultType.UserCancel
                };

                window.Closed += (o, e) =>
                {
                    signal.Release();
                };
                browser.DOMContentLoaded += (sender, args) =>
                {
                    if (args.Uri.ToString().StartsWith(options.EndUrl))
                    {
                        result.ResultType = BrowserResultType.Success;

                        result.Response = args.Uri.ToString();

                        signal.Release();
                    }
                };

                grid.Children.Add(browser);
                window.Show();

                browser.Navigate(options.StartUrl);

                await signal.WaitAsync();

                if (!_shouldCloseWindow)
                {
                    grid.Children.Clear();
                }

                return result;
            }
            finally
            {
                if (_shouldCloseWindow)
                {
                    window.Close();
                }
            }
        }
    }
}
