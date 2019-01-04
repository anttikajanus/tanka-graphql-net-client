using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tanka.GraphQL
{
    /// <summary>
    /// Result that is returned from the server.
    /// </summary>
    public class ExecutionResult
    {
        private IDictionary<string, object> _data;
        private IDictionary<string, object> _extensions;
        private IEnumerable<GraphQLError> _errors;

        /// <summary>
        /// Returned data as a dictionary. The key is the operation name of the query.
        /// </summary>
        public IDictionary<string, object> Data
        {
            get => _data;
            set
            {
                if (value != null && !value.Any())
                {
                    _data = null;
                    return;
                }

                _data = value;
            }
        }

        /// <summary>
        /// Returns a dictionary of special fields, that were returned in the GraphQL response, that allows server to attach extra metadata. 
        /// </summary>
        /// <remarks>
        /// Tanka GraphQL execution engine might return a tracing information to the client using special "tracing" key. This data is following
        /// Apollo tracing format that is exposed as a <see cref="Tracing.TraceExtensionRecord"/> object. 
        /// </remarks>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IDictionary<string, object> Extensions
        {
            get => _extensions;
            set
            {
                if (value != null && !value.Any())
                {
                    _extensions = null;
                    return;
                }

                _extensions = value;
            }
        }

        /// <summary>
        /// Returned errors.
        /// </summary>
        /// <remarks>
        /// Note that the repsonse might return both <see cref="Data"/> and <see cref="Errors"/> in the same result.
        /// </remarks>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<GraphQLError> Errors
        {
            get => _errors;
            set
            {
                if (value != null && !value.Any())
                {
                    _errors = null;
                    return;
                }

                _errors = value;
            }
        }

        /// <summary>
        /// Get a field from the <see cref="Extensions"/> as a concrete type.
        /// </summary>
        /// <typeparam name="TFieldType">The expected type.</typeparam>
        /// <param name="fieldName">The name of the field.</param>
        /// <returns>The data from the field as an object.</returns>
        public TFieldType GetExtensionFieldAs<TFieldType>(string fieldName)
        {
            if (!Extensions.ContainsKey(fieldName))
                throw new KeyNotFoundException($"Cannot find {fieldName} from the returned {nameof(Extensions)}.");

            var json = Extensions[fieldName].ToString();
            var value = JsonConvert.DeserializeObject<TFieldType>(json);
            return value;
        }

        /// <summary>
        /// Get a field from the <see cref="Data"/> as a concrete type. If field parameter is not defined, first value is used.
        /// </summary>
        /// <typeparam name="TFieldType">The expected type.</typeparam>
        /// <param name="fieldName">The name of the field.</param>
        /// <returns>The data from the field as an object.</returns>
        public TFieldType GetDataFieldAs<TFieldType>(string fieldName = "")
        {
            if (Data == null)
                throw new Exception("Data wasn't returned from the server...");
            var value = string.Empty;
            if (string.IsNullOrEmpty(fieldName) || string.IsNullOrWhiteSpace(fieldName))
            {
                value = Data.First().Value.ToString();
            }
            else
            {
                value = Data[fieldName].ToString();
            }

            var result = JsonConvert.DeserializeObject<TFieldType>(value);
            return result;
        }
    }
}
