﻿namespace BinaryPack.Serialization.Processors
{
    internal sealed partial class ObjectProcessor<T>
    {
        /// <summary>
        /// A <see langword="class"/> that exposes hardcoded indices for local variables
        /// </summary>
        private static class Locals
        {
            /// <summary>
            /// An <see langword="enum"/> with a collection of local variables used during deserialization
            /// </summary>
            public enum Read
            {
                /// <summary>
                /// The input instance
                /// </summary>
                T
            }
        }
    }
}
