using System;

namespace Tanka.GraphQL.Sample.Chat.Client.Shared
{
    public interface IDispatcherContext
    {
        bool IsSynchronized { get; }
        void Invoke(Action action);
        void BeginInvoke(Action action);
    }
}
