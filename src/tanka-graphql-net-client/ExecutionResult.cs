using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Tanka.GraphQL
{
    /// <summary>
    /// Result that is returned from the server.
    /// </summary>
    public class ExecutionResult
    {
        private IDictionary<string, object> _data;
        private IDictionary<string, object> _extensions;
        private IEnumerable<ExecutionError> _errors;

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
        public IEnumerable<ExecutionError> Errors
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
        
        private TFieldType GetFieldAs<TFieldType>(IDictionary<string, object> dictionary, string key)
        {
            string value;
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                value = (string)dictionary.First().Value;
            else
                value = dictionary[key].ToString();

            if (value == null)
                throw new FormatException("test this... since errors are returned but also this key is there with null value.");

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
           
            var result = JsonSerializer.Deserialize<TFieldType>(value, options);
            //var x = dictionary.First().Value;
            //foreach (var item in x as IList<object>)
            //{
            //    Debug.WriteLine(item.ToString());
            //    Debug.WriteLine(item);
            //    //foreach (var item2 in item)
            //    //{

            //    //}
            //}


            //Debug.WriteLine(value.GetType().ToString());
            //foreach (var item in value as List<object>)
            //{
            //    Debug.WriteLine(item.GetType().ToString());
            //    Debug.WriteLine(item);

            //}
            return result;
            //var result = JsonSerializer.Deserialize<TFieldType>(value);
            //var result = JsonConvert.DeserializeObject<TFieldType>(value);
           // return result;
        }
    }
}
