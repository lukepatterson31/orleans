using System;

namespace Orleans.Runtime
{
    /// <summary>
    /// Identifies a client.
    /// </summary>
    internal readonly struct ClientGrainId : IEquatable<ClientGrainId>, IComparable<ClientGrainId>
    {
        /// <summary>
        /// Creates a new <see cref="ClientGrainId"/> instance.
        /// </summary>
        private ClientGrainId(GrainId grainId) => this.GrainId = grainId;

        /// <summary>
        /// Gets the underlying <see cref="GrainId"/>.
        /// </summary>
        public GrainId GrainId { get; }

        /// <summary>
        /// Creates a new <see cref="ClientGrainId"/> instance.
        /// </summary>
        public static ClientGrainId Create() => Create(GrainIdKeyExtensions.CreateGuidKey(Guid.NewGuid()));

        /// <summary>
        /// Creates a new <see cref="ClientGrainId"/> instance.
        /// </summary>
        public static ClientGrainId Create(string id) => Create(IdSpan.Create(id));

        /// <summary>
        /// Creates a new <see cref="ClientGrainId"/> instance.
        /// </summary>
        public static ClientGrainId Create(IdSpan id) => new ClientGrainId(new GrainId(GrainTypePrefix.ClientGrainType, id));

        /// <summary>
        /// Converts the provided <see cref="GrainId"/> to a <see cref="ClientGrainId"/>. A return value indicates whether the operation succeeded.
        /// </summary>
        public static bool TryParse(GrainId grainId, out ClientGrainId clientId)
        {
            if (!grainId.Type.IsClient())
            {
                clientId = default;
                return false;
            }

            // Strip the observer id, if present.
            var key = grainId.Key.AsSpan();
            if (key.IndexOf((byte)ObserverGrainId.SegmentSeparator) is int index && index >= 0)
            {
                key = key.Slice(0, index);
                grainId = new GrainId(grainId.Type, new IdSpan(key.ToArray()));
            }

            clientId = new ClientGrainId(grainId);
            return true;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is ClientGrainId clientId && this.GrainId.Equals(clientId.GrainId);

        /// <inheritdoc/>
        public override int GetHashCode() => this.GrainId.GetHashCode();

        /// <inheritdoc/>
        public override string ToString() => this.GrainId.ToString();

        /// <inheritdoc/>
        public bool Equals(ClientGrainId other) => this.GrainId.Equals(other.GrainId);

        /// <inheritdoc/>
        public int CompareTo(ClientGrainId other) => this.GrainId.CompareTo(other.GrainId);

        /// <summary>
        /// Compares the provided operands for equality.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns><see langword="true"/> if the provided values are equal, otherwise <see langword="false"/>.</returns>
        public static bool operator ==(ClientGrainId left, ClientGrainId right) => left.Equals(right);

        /// <summary>
        /// Compares the provided operands for inequality.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns><see langword="true"/> if the provided values are not equal, otherwise <see langword="false"/>.</returns>
        public static bool operator !=(ClientGrainId left, ClientGrainId right) => !(left == right);
    }
}
