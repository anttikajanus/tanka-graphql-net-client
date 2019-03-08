using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Tanka.GraphQL.Sample.Chat.Client.Shared
{
    public abstract class SubscriptionBase<TResult>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SubscriptionBase{TResult}"/> that provides execution support for GraphQL subscriptions.
        /// </summary>
        /// <param name="connection">Connection that is used to execute the query with.</param>
        protected SubscriptionBase(HubConnection connection)
        {
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        /// <summary>
        /// Connection that is used to execute the query with. 
        /// </summary>
        protected HubConnection Connection { get; }

        /// <summary>
        /// Subsusing the provided <see cref="Connection"/>.
        /// </summary>
        /// <param name="request">Query parameters.</param>
        /// <returns>Returns execution results as a concrete type.</returns>
        protected async Task<IObservable<TResult>> SubscribeAsync(QueryRequest request, CancellationToken token)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            // Project returned stream to TResult. Assumes that we are taking the first returned item from the
            // returned data.
            var subscription = (await Connection.SubscribeAsync(request, token))
                .Select(i => i.GetDataFieldAs<TResult>());
            return subscription;
        }
    }
}
