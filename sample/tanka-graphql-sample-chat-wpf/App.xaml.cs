using Prism.Ioc;
using Prism.Ninject;
using System.Configuration;
using System.Windows;
using Tanka.GraphQL.Sample.Chat.Client.Shared;
using Tanka.GraphQL.Sample.Chat.Client.Shared.Services;
using Tanka.GraphQL.Sample.Chat.Client.Wpf.Services;
using Tanka.GraphQL.Sample.Chat.Client.Wpf.Views;

namespace Tanka.GraphQL.Sample.Chat.Client.Wpf
{
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            var shell = Container.Resolve<MainWindow>();
            if (shell.DataContext != null && shell.DataContext is IAsyncInitializer)
            {
                (shell.DataContext as IAsyncInitializer).InitializeAsync(ConfigurationManager.AppSettings["Tanka:SignalRHubUrl"]);
            }

            return shell;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAuthenticationService, AuthenticationService>();
            containerRegistry.RegisterSingleton<IDispatcherContext, WpfDispatcherContext>();
            containerRegistry.RegisterSingleton<IChatService, ChatService>();
        }

        protected override void ConfigureServiceLocator()
        {
            base.ConfigureServiceLocator();
        }
    }
}
