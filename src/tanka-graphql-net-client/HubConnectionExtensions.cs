﻿using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
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
        /// <param name="query">Query that will be executed.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the query.</param>
        /// <returns>Return <see cref="GraphQLExecutionResult"/> that contains the execution result from the service.</returns>
        /// <remarks>
        /// Use <see cref="QueryAsync(HubConnection, QueryRequest)"/> for queries and mutations and use 
        /// <see cref="SubscribeAsync(HubConnection, QueryRequest)"/> for subscriptions.
        /// </remarks>
        public static async Task<ExecutionResult> QueryAsync(
            this HubConnection connection, QueryRequest query, CancellationToken cancellationToken = default)
        {
            // read from the hub using async streams
            var stream = connection.StreamAsync<ExecutionResult>("query", query, cancellationToken);
            await foreach (var result in stream)
            {
                return result;
            }

            //Todo: Create new exception type for graphql exceptions
            throw new Exception($"Query operation failed to be executed. Operation name = {query.OperationName}, Query = {query.Query}");

            //var channelReader = await connection.StreamAsChannelAsync<ExecutionResult>("query", queryRequest, cancellationToken);

                //while (await channelReader.WaitToReadAsync(cancellationToken))
                //{
                //    while (channelReader.TryRead(out var result))
                //    {
                //        return result;
                //    }
                //}

        }

        /// <summary>
        /// Subscribe to a exposed GraphQL subscription using the <see cref="HubConnection"/>.
        /// </summary>
        /// <param name="connection">SignalR connection that is used to execute the query.</param>
        /// <param name="queryRequest">Subscription query that will be executed.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the subscription.</param>
        /// <returns>
        /// Returns <see cref="IObservable{GraphQLExecutionResult}"/> that pushes notificatons to the subscribers when subscribed event occurs on the server.
        /// </returns>
        /// <remarks>
        /// Use <see cref="QueryAsync(HubConnection, QueryRequest)"/> for queries and mutations and use 
        /// <see cref="SubscribeAsync(HubConnection, QueryRequest)"/> for subscriptions.
        /// </remarks>
        public static async Task<IObservable<ExecutionResult>> SubscribeAsync(
            this HubConnection connection, QueryRequest query, CancellationToken cancellationToken = default)
        {
            //var channelReader = await connection.StreamAsChannelAsync<ExecutionResult>("query", query, cancellationToken);
            var subject = new Subject<ExecutionResult>();
            
            var subscriptionTask = Task.Run(async () =>
            {
                try
                {
                    // read from the hub using async streams
                    var stream = connection.StreamAsync<ExecutionResult>("query", query, cancellationToken);
                    await foreach (var result in stream)
                    {
                        subject.OnNext(result);
                    }
                    //while (await channelReader.WaitToReadAsync(cancellationToken))
                    //{
                    //    while (channelReader.TryRead(out var result))
                    //    {
                    //    }
                    //}
                }
                catch (OperationCanceledException)
                {
                    // Operation cancelled, eat exception here and close the stream after this
                }
                catch (Exception ex)
                {
                    subject.OnError(ex);
                }
                finally
                {
                    subject.OnCompleted();
                }
            }, cancellationToken);

            return subject.AsObservable();
        }
    }
}
