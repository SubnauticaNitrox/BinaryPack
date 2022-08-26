using System;

namespace BinaryPack.Attributes
{
    /// <summary>
    /// A custom <see cref="Attribute"/> that marks a constructor so it will never be used for serialization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor)]
    public sealed class IgnoreConstructorAttribute : Attribute { }
}
