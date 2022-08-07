using System;

namespace BinaryPack.Attributes
{
    /// <summary>
    /// A custom <see cref="Attribute"/> that marks a constructor so it will be used for serialization if no parameterless constructor is available.
    /// This ignores all checks if parameter and member count are equal.
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor)]
    public sealed class ForceUseConstructorAttribute : Attribute { }
}
