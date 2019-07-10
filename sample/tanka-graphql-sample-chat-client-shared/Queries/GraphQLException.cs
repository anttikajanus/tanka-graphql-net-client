using System;
using System.Collections.Generic;
using System.Text;

namespace Tanka.GraphQL.Sample.Chat.Client.Shared.Queries
{
    public class GraphQLException : Exception
    {
        public GraphQLException(string message) : base(message)
        {
        }
    }
}
