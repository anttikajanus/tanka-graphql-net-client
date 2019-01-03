using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace Tanka.GraphQL
{
    /// <summary>
    /// Contains extension methods to execute GraphQL queries against Tanka GraphQL execution engine.
    /// </summary>
    public static class HubConnectionExtensions
    {
        /// <summary>
        /// Executes a provided <see cref="QueryRequest"/> using the <see cref="HubConnection"/>.
        /// </summary>
        /// <param name="connection">SignalR connection that is used to execute the query.</param>
        /// <param name="queryRequest">Query that will be executed.</param>
        /// <returns>Return <see cref="GraphQLExecutionResult"/> that contains the execution result from the service.</returns>
        /// <remarks>
        /// Use <see cref="QueryAsync(HubConnection, QueryRequest)"/> for queries and mutations and use 
        /// <see cref="SubscribeAsync(HubConnection, QueryRequest)"/> for subscriptions.
        /// </remarks>
        public static async Task<ExecutionResult> QueryAsync(this HubConnection connection, QueryRequest queryRequest)
        {
            var channelReader = await connection.StreamAsChannelAsync<ExecutionResult>("query", queryRequest);

            while (await channelReader.WaitToReadAsync())
            {
                while (channelReader.TryRead(out var result))
                {
                    return result;
                }
            }

            throw new Exception($"Query operation failed to be executed. Operation name = {queryRequest.OperationName}, Query = {queryRequest.Query}");
        }

        /// <summary>
        /// Subscribe to a exposed GraphQL subscription using the <see cref="HubConnection"/>.
        /// </summary>
        /// <param name="connection">SignalR connection that is used to execute the query.</param>
        /// <param name="queryRequest">Subscription query that will be executed.</param>
        /// <returns>
        /// Returns <see cref="IObservable{GraphQLExecutionResult}"/> that pushes notificatons to the subscribers when subscribed event occurs on the server.
        /// </returns>
        /// <remarks>
        /// Use <see cref="QueryAsync(HubConnection, QueryRequest)"/> for queries and mutations and use 
        /// <see cref="SubscribeAsync(HubConnection, QueryRequest)"/> for subscriptions.
        /// </remarks>
        public static async Task<IObservable<ExecutionResult>> SubscribeAsync(this HubConnection connection, QueryRequest query)
        {
            var channelReader = await connection.StreamAsChannelAsync<ExecutionResult>("query", query);
            var subject = new Subject<ExecutionResult>();

            var subscriptionTask = Task.Run(async () =>
            {
                try
                {
                    while (await channelReader.WaitToReadAsync())
                    {
                        while (channelReader.TryRead(out var result))
                        {
                            subject.OnNext(result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    subject.OnError(ex);
                }
                finally
                {
                    subject.OnCompleted();
                }
            });

            return subject.AsObservable();
        }
    }
}
