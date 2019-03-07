using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Tanka.GraphQL.Sample.Chat.Client.Shared
{
    /// <summary>
    /// A base class to provide support to execute GraphQL queries
    /// </summary>
    /// <typeparam name="TResult">Type of the result returned from the GraphQL query.</typeparam>
    /// <remarks>
    /// Use <see cref="ExecuteQueryAsync(GraphQLQueryRequest)"/> to execute both queries and mutations.
    /// Use <see cref="SubscribeAsync(GraphQLQueryRequest)"/> with subscriptions.
    /// </remarks>
    public abstract class QueryBase<TResult>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="QueryBase{TResult}"/> that provides execution support for GraphQL queries.
        /// </summary>
        /// <param name="connection">Connection that is used to execute the query with.</param>
        protected QueryBase(HubConnection connection)
        {
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        /// <summary>
        /// Connection that is used to execute the query with. 
        /// </summary>
        protected HubConnection Connection { get; }

        /// <summary>
        /// Execute query using the provided <see cref="Connection"/>.
        /// </summary>
        /// <param name="request">Query parameters.</param>
        /// <returns>Returns execution results as a concrete type.</returns>
        protected async Task<TResult> ExecuteQueryAsync(QueryRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var result = await Connection.QueryAsync(request);


            //var json = JsonConvert.SerializeObject(result);
            //Debug.WriteLine(json);

            var data = result.GetDataFieldAs<TResult>();
            return data;
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
