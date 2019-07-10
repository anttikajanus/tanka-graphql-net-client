using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanka.GraphQL.Sample.Chat.Client.Shared.Queries;

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
            if (result.Errors != null && result.Errors.Any())
            {
                var messageBuilder = new StringBuilder();
                messageBuilder.AppendLine($"GraphQL query returned {result.Errors.Count()} errors:");
                foreach (var error in result.Errors)
                {
                    messageBuilder.AppendLine(error.Message);
                }
                throw new GraphQLException(messageBuilder.ToString());
            }

            var data = result.GetDataFieldAs<TResult>();
            return data;
        }
    }
}
