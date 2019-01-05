using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tanka.GraphQL.Net.Client.Tests.Helpers
{
    public static class ExecutionResultHelper
    {
        /// <summary>
        /// Use to create objects simalarly to SignalR hub serialization
        /// </summary>
        /// <param name="json">Response Json</param>
        /// <returns>Returns <see cref="ExecutionResult"/> based on the provided json.</returns>
        public static ExecutionResult CreateFromJson(string json)
        {
            return JsonConvert.DeserializeObject<ExecutionResult>(json);
        }
    }
}
