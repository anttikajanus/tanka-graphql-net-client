using System;
using System.Collections.Generic;

namespace Tanka.GraphQL
{
    /// <summary>
    /// Represents the location where the <see cref="ExecutionError"/> has been found
    /// </summary>
    public sealed class Location : IEquatable<Location>
    {
        /// <summary>
        /// The Column
        /// </summary>
        public uint Column { get; set; }

        /// <summary>
        /// The Line
        /// </summary>
        public uint Line { get; set; }

        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as Location);

        /// <inheritdoc />
        public bool Equals(Location other)
        {
            if (other == null)
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            if (!Equals(this.Column, other.Column))
            {
                return false;
            }
            if (!Equals(this.Line, other.Line))
            {
                return false;
            }
            return true;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hashCode = 412437926;
            hashCode = hashCode * -1521134295 + Column.GetHashCode();
            hashCode = hashCode * -1521134295 + Line.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc />
        public static bool operator ==(Location location1, Location location2) => EqualityComparer<Location>.Default.Equals(location1, location2);

        /// <inheritdoc />
        public static bool operator !=(Location location1, Location location2) => !(location1 == location2);
    }
}
