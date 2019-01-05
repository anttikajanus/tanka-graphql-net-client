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
        /// Get an item from the <see cref="Extensions"/> as a concrete type. If key parameter is not defined or is empty, first item is used.
        /// </summary>
        /// <typeparam name="TFieldType">The expected type.</typeparam>
        /// <param name="key">The key of the field.</param>
        /// <returns>The data from the item as an object.</returns>
        /// <exception cref="InvalidOperationException">Thrown if result doesn't contain any <see cref="Extensions"/></exception>
        /// <exception cref="KeyNotFoundException">Thrown if <see cref="Extensions"/> didn't have item with a given key</exception>
        public TFieldType GetExtensionFieldAs<TFieldType>(string key = "")
        {
            if (Extensions== null)
                throw new InvalidOperationException($"{this} doesn't contain any extensions.");

            var result = GetFieldAs<TFieldType>(Extensions, key);
            return result;
        }

        /// <summary>
        /// Get an item from the <see cref="Data"/> as a concrete type. If key parameter is not defined or is empty, first item is used.
        /// </summary>
        /// <typeparam name="TFieldType">The expected type.</typeparam>
        /// <param name="key">The key of the item.</param>
        /// <returns>The data from the item as an object.</returns>
        /// <exception cref="InvalidOperationException">Thrown if result doesn't contain any <see cref="Data"/></exception>
        /// <exception cref="KeyNotFoundException">Thrown if <see cref="Data"/> didn't have item with a given key</exception>
        public TFieldType GetDataFieldAs<TFieldType>(string key = "")
        {
            if (Data == null)
                throw new InvalidOperationException($"{this} doesn't contain any data.");

            var result = GetFieldAs<TFieldType>(Data, key);
            return result;
        }

        private TFieldType GetFieldAs<TFieldType>(IDictionary<string, object> dictinary, string key)
        {
            var value = string.Empty;
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                value = dictinary.First().Value.ToString();
            else
                value = dictinary[key].ToString();

            var result = JsonConvert.DeserializeObject<TFieldType>(value);
            return result;
        }
    }
}
