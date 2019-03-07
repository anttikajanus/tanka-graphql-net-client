using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
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

            var subscription = (await Connection.SubscribeAsync(request, token))
                .Select(i => i.GetDataFieldAs<TResult>());
            return subscription;
        }

        /// <summary>
        /// Subscribe to a exposed GraphQL subscription. 
        /// </summary>
        /// <param name="request">Subscription parameters.</param>
        /// <returns>Returns <see cref="IObservable{T}"/> that pushes notificatons to the subscribers when subscribed event occurs on the server.
        /// The pushed type is the type returned by the subscription.
        /// </returns>
        //protected async Task<IObservable<TResult>> SubscribeAsync(QueryRequest request)
        //{ 
        //    if (request == null)
        //        throw new ArgumentNullException(nameof(request));

        //    // Use intermidiate subject to transform the result to the transfer object
        //    var subject = new Subject<TResult>();
        //    var serverSubscription = await Connection.SubscribeAsync(request);
        //    serverSubscription.Subscribe(
        //        // New event
        //        result =>
        //        {
        //            TResult message = result.GetDataFieldAs<TResult>();
        //            subject.OnNext(message);
        //        },
        //        // On error
        //        error =>
        //        {
        //            subject.OnError(error);
        //        },
        //        // On completed
        //        () =>
        //        {
        //            subject.OnCompleted();
        //        });
        //    return subject.AsObservable();
        //}
    }
}
