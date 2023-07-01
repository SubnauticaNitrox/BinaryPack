using System;

namespace BinaryPack.Attributes
{
    /// <summary>
    /// A custom <see cref="Attribute"/> that marks a <see langword="class"/> so it is guaranteed to have a it's type information serialized.
    /// Needs to have an abstract base <see langword="class"/> that is registered with <see cref="BinaryConverter.RegisterUnion"/>.
    /// Does not work if the type of the variable (not the runtime object) is a sub-<see langword="class"/> of the <see langword="class"/> with this attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ForceUnionAttribute : Attribute { }
}
