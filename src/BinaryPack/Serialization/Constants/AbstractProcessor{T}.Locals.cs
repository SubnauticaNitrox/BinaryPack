namespace BinaryPack.Serialization.Processors;

internal sealed partial class AbstractProcessor<TBase>
{
    /// <summary>
    /// A <see langword="class"/> that exposes local variables for <see cref="AbstractProcessor{TBase}"/>
    /// </summary>
    private static class Locals
    {
        /// <summary>
        /// An <see langword="enum"/> with a collection of local variables used during deserialization
        /// </summary>
        public enum Read
        {
            /// <summary>
            /// The serialized index of the serialized subclass type in the union types array
            /// </summary>
            UnionIndex
        }
    }
}
