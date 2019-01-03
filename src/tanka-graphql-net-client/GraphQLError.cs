﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tanka.GraphQL
{
    /// <summary>
    /// Represents an error returned that happened on the server while executing GraphQL query.
    /// </summary>
    public class GraphQLError
    {
        public GraphQLError(string message)
        {
            Message = message;
        }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets the list of locations involved with the error.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Location> Locations { get; set; }

        /// <summary>
        /// Gets the path to the error
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<object> Path { get; set; }

        /// <summary>
        /// Returns a dictionary of special fields, that were returned in the GraphQL response, that allows server to attach extra metadata. 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> Extensions { get; set; }
    }
}
