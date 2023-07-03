using System;
using System.Linq;

#nullable enable

namespace BinaryPack.Models
{
    /// <summary>
    /// A simple model that contains data of dynamic size
    /// </summary>
    [Serializable]
    public sealed class DynamicSizeModel : IEquatable<DynamicSizeModel>
    {
        public int[] IntValues { get; set; }
        public bool[] BoolValues { get; set; }

        public void Initialize(int size)
        {
            int sizeInts = size / 4;
            int sizeBooleans = size % 4;

            IntValues = new int[sizeInts];
            BoolValues = new bool[sizeBooleans];

            Random rnd = new Random();
            for (int i = 0; i < sizeInts; i++)
            {
                IntValues[i] = rnd.Next();
            }
            for (int i = 0; i < sizeBooleans; i++)
            {
                BoolValues[i] = rnd.NextDouble() >= 0.5;
            }
        }

        /// <inheritdoc/>
        public bool Equals(DynamicSizeModel? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return IntValues.SequenceEqual(other.IntValues) && BoolValues.SequenceEqual(other.BoolValues);
        }
    }
}
