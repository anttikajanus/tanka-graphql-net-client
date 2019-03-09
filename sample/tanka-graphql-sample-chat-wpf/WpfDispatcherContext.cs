using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Threading;
using Tanka.GraphQL.Sample.Chat.Client.Shared;

namespace Tanka.GraphQL.Sample.Chat.Client.Wpf
{
    public class WpfDispatcherContext : IDispatcherContext
    {
        private readonly Dispatcher _dispatcher;

        public bool IsSynchronized => _dispatcher.Thread == Thread.CurrentThread;

        public WpfDispatcherContext() : this(Dispatcher.CurrentDispatcher)
        {
        }

        public WpfDispatcherContext(Dispatcher dispatcher)
        {
            Debug.Assert(dispatcher != null);
            _dispatcher = dispatcher;
        }

        public void Invoke(Action action)
        {
            Debug.Assert(action != null);
            _dispatcher.Invoke(action);
        }

        public void BeginInvoke(Action action)
        {
            Debug.Assert(action != null);
            _dispatcher.BeginInvoke(action);
        }
    }
}
