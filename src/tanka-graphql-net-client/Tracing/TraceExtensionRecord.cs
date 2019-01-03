using System;
using System.Collections.Generic;
using System.Text;

namespace Tanka.GraphQL.Tracing
{
    /// <summary>
    /// Describes a trace record that might be returned from the server as part of the <see cref="ExecutionResult.Extensions"/>  with a special key
    /// "tracing". The data is following "apollo-tracing" format. Read more about the format from https://github.com/apollographql/apollo-tracing.
    /// </summary>
    public class TraceExtensionRecord
    {
        /// <summary>
        /// Version of the tracing infrastructure.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Start time of the request.
        /// </summary>
        /// <remarks>Up to the nanosecond accuracy if the platform supports it.</remarks>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// End time of the request.
        /// </summary>
        /// <remarks>Up to the nanosecond accuracy if the platform supports it.</remarks>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Duration of the operation in nanoseconds relative to the request start (<see cref="StartTime"/>.
        /// </summary>
        public long Duration { get; set; }

        /// <summary>
        /// Tracing information for parsing on the server.
        /// </summary>
        public OperationTrace Parsing { get; set; }

        /// <summary>
        /// Tracing information for validation on the server.
        /// </summary>
        public OperationTrace Validation { get; set; }

        /// <summary>
        /// Resolver execution trace information.
        /// </summary>
        public ExecutionTrace Execution { get; set; } = new ExecutionTrace();

        /// <summary>
        /// Operation trace
        /// </summary>
        public class OperationTrace
        {
            /// <summary>
            /// The StartOffset of parsing, validation, or a resolver call is in nanoseconds, relative to the request start.
            /// </summary>
            /// <seealso cref="StartTime"/>
            public long StartOffset { get; set; }

            /// <summary>
            /// The duration of parsing, validation, or a resolver call is in nanoseconds, relative to the operation/resolver call start.
            /// </summary>
            public long Duration { get; set; }
        }

        /// <summary>
        /// Execution trace contains tracing iformation for the run resolvers.
        /// </summary>
        public class ExecutionTrace
        {
            public List<ResolverTrace> Resolvers { get; set; } = new List<ResolverTrace>();
        }

        /// <summary>
        /// Resolver trace
        /// </summary>
        public class ResolverTrace : OperationTrace
        {
            /// <summary>
            /// The path is the response path of the current resolver in a format similar to the error result format specified in the GraphQL specification
            /// </summary>
            public List<object> Path { get; set; } = new List<object>();

            /// <summary>
            /// ParentType reflects the runtime type information usually passed to resolvers.
            /// </summary>
            public string ParentType { get; set; }

            /// <summary>
            /// FieldName reflects the runtime type information usually passed to resolvers.
            /// </summary>
            public string FieldName { get; set; }

            /// <summary>
            /// ReturnType reflects the runtime type information usually passed to resolvers.
            /// </summary>
            public string ReturnType { get; set; }
        }
    }
}
