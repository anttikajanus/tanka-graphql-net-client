using Newtonsoft.Json;
using System.Collections.Generic;
using Tanka.GraphQL.Utilities;

namespace Tanka.GraphQL
{
    /// <summary>
    /// <see cref="QueryRequest"/> represents a operation that is executed on the GraphQL service.
    /// </summary>
    /// <remarks>
    /// In GraphQL terminology query, mutation and subscription are seen as queries.
    /// </remarks>
    public class QueryRequest
    {
        /// <summary>
        /// Query that is executed on the GraphQL server.
        /// </summary>
        /// <remarks>
        /// In GraphQL terminology query, mutation and subscription are seen as Queries.
        /// Read more from https://graphql.github.io/learn/queries/
        /// </remarks>
        public string Query { get; set; }

        /// <summary>
        /// A collection of variables (or parameters) for the query.
        /// </summary>
        /// <remarks>
        /// In GraphQL dynamic variables (or parameters) are define out of the query as a separate dictionary. 
        /// Read more from https://graphql.github.io/learn/queries/#variables
        /// </remarks>
        [JsonConverter(typeof(VariableConverter))]
        public Dictionary<string, object> Variables { get; set; }

        /// <summary>   
        /// A name for a single query, mutation, or subscription. Identifying a query or mutation by name is very useful for 
        /// logging and debugging when something goes wrong in a GraphQL server.
        /// </summary>>
        /// <remarks>
        /// Read more from https://graphql.github.io/learn/queries/#operation-name
        /// </remarks>
        public string OperationName { get; set; }

        /// <summary>
        /// A dictionary of special fields, that are sent to the Graphql execution.  
        /// </summary>
        /// <remarks>
        /// Fugu GraphQL execution engine uses <see cref="Extensions"/> to deliver arbitary information as part of your queries. 
        /// This is not strigtly defined int he GraphQL spec but is used by many GraphQL implementations as a community standard.
        /// </remarks>
        [JsonConverter(typeof(VariableConverter))]
        public Dictionary<string, object> Extensions { get; set; }
    }
}
