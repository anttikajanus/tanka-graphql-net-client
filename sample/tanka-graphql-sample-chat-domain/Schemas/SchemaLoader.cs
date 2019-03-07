using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using tanka.graphql;
using tanka.graphql.sdl;
using tanka.graphql.type;

namespace Tanka.GraphQL.Sample.Chat.Domain.Schemas
{
    public static class SchemaLoader
    {
        /// <summary>
        /// Loads the schema
        /// </summary>
        /// <returns></returns>
        public static async Task<ISchema> LoadAsync()
        {
            var idl = await LoadIdlFromResourcesAsync();
            var schema = Sdl.Schema(Parser.ParseDocument(idl));

            return schema;
        }

        /// <summary>
        /// Load schema from embedded resource
        /// </summary>
        /// <returns></returns>
        private static async Task<string> LoadIdlFromResourcesAsync()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceStream =
                assembly.GetManifestResourceStream("Tanka.GraphQL.Sample.Chat.Domain.Schemas.Chat.graphql");
            using (var reader =
                new StreamReader(resourceStream ?? throw new InvalidOperationException(), Encoding.UTF8))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}