using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using BinaryPack.Attributes;
using BinaryPack.Serialization.Processors;
using BinaryPack.Serialization.Processors.Abstract;
using BinaryPack.Serialization.Processors.Arrays;
using BinaryPack.Serialization.Processors.Collections;

namespace BinaryPack.Serialization.Reflection
{
    internal static partial class KnownMembers
    {
        /// <summary>
        /// A <see langword="class"/> containing methods from types inherited from <see cref="Processors.Abstract.TypeProcessor{T}"/>
        /// </summary>
        public static class TypeProcessor
        {
            /// <summary>
            /// Gets the <see cref="MethodInfo"/> instance for a dynamic serialization method for a given type
            /// </summary>
            /// <param name="objectType">The type of the item being handled by the requested processor</param>
            /// <param name="name">The name of property to retrieve from the processor instance</param>
            /// <param name="ignoreInheritedAbstract">Prevents non abstract classes with [<see cref="ForceUnionAttribute"/>] to be matched to an <see cref="AbstractProcessor{TBase}"/>. This prevents cyclic execution.</param>
            [Pure]
            private static MethodInfo GetMethodInfo(Type objectType, string name, bool ignoreInheritedAbstract)
            {
                static bool IsInheritedAbstractEligible(Type objectType) => objectType.GetCustomAttributes<ForceUnionAttribute>(false).Any() &&
                                                                            objectType.GetAbstractBaseType() != null;

                /* Get the right processor type for the input object type.
                 * Note that not all possible types are supported here. For instance,
                 * generic interfaces like IList<T> require special handling during
                 * the serialization and deserialization, which is not limited to
                 * just the use of a specific processor. For now, those case
                 * are just marked as not supported. */
                Type processorType = objectType switch
                {
                    _ when objectType.IsGenericType(typeof(Nullable<>)) => typeof(NullableProcessor<>).MakeGenericType(objectType.GenericTypeArguments[0]),
                    _ when objectType == typeof(string) => typeof(StringProcessor),
                    _ when objectType.IsSZArray() => typeof(SZArrayProcessor<>).MakeGenericType(objectType.GetElementType()),
                    _ when objectType.IsArray && objectType.GetArrayRank() > 1 => typeof(ArrayProcessor<>).MakeGenericType(objectType),
                    _ when objectType.IsGenericType(typeof(List<>)) => typeof(ListProcessor<>).MakeGenericType(objectType.GenericTypeArguments[0]),
                    _ when objectType.IsGenericType(typeof(ICollection<>)) => typeof(ICollectionProcessor<>).MakeGenericType(objectType.GenericTypeArguments[0]),
                    _ when objectType.IsGenericType(typeof(IReadOnlyCollection<>)) => typeof(IReadOnlyCollectionProcessor<>).MakeGenericType(objectType.GenericTypeArguments[0]),
                    _ when objectType.IsGenericType(typeof(IEnumerable<>)) => typeof(IEnumerableProcessor<>).MakeGenericType(objectType.GenericTypeArguments[0]),
                    _ when objectType.IsGenericType(typeof(Dictionary<,>)) => typeof(DictionaryProcessor<,>).MakeGenericType(objectType.GenericTypeArguments),
                    _ when objectType.IsGenericType(typeof(IDictionary<,>)) => typeof(IDictionaryProcessor<,>).MakeGenericType(objectType.GenericTypeArguments),
                    _ when objectType.IsGenericType(typeof(IReadOnlyDictionary<,>)) => typeof(IReadOnlyDictionaryProcessor<,>).MakeGenericType(objectType.GenericTypeArguments),
                    _ when objectType == typeof(BitArray) => typeof(BitArrayProcessor),
                    _ when objectType.IsAbstract || objectType.IsInterface => typeof(AbstractProcessor<>).MakeGenericType(objectType),
                    _ when !ignoreInheritedAbstract && IsInheritedAbstractEligible(objectType) => typeof(AbstractProcessor<>).MakeGenericType(objectType.GetAbstractBaseType()),
                    _ => typeof(ObjectProcessor<>).MakeGenericType(objectType)
                };

                // Access the static TypeProcessor<T> instance to get the requested dynamic method
                PropertyInfo instanceInfo = processorType.GetProperty(nameof(StringProcessor.Instance)); // Guaranteed to be there for all processors
                object processorInstance = instanceInfo.GetValue(null);
                PropertyInfo dynamicMethodInfo = processorType.GetProperty(name);
                object genericMethod = dynamicMethodInfo.GetValue(processorInstance);
                PropertyInfo methodInfoInfo = genericMethod.GetType().GetProperty(nameof(DynamicMethod<Action>.MethodInfo));

                return (MethodInfo)methodInfoInfo.GetValue(genericMethod);
            }

            /// <summary>
            /// Gets the <see cref="MethodInfo"/> instance for the dynamic serializer of a given type
            /// </summary>
            /// <param name="objectType">The type of the item being handled by the requested processor</param>
            /// <param name="ignoreInheritedAbstract">Prevents non abstract classes with [<see cref="ForceUnionAttribute"/>] to be matched to an <see cref="AbstractProcessor{TBase}"/>. This prevents cyclic execution.</param>
            [Pure]
            public static MethodInfo SerializerInfo(Type objectType, bool ignoreInheritedAbstract = false) => GetMethodInfo(objectType, nameof(TypeProcessor<object>.SerializerInfo), ignoreInheritedAbstract);

            /// <summary>
            /// Gets the <see cref="MethodInfo"/> instance for the dynamic deserializer of a given type
            /// </summary>
            /// <param name="objectType">The type of the item being handled by the requested processor</param>
            /// <param name="ignoreInheritedAbstract">Prevents non abstract classes with [<see cref="ForceUnionAttribute"/>] to be matched to an <see cref="AbstractProcessor{TBase}"/>. This prevents cyclic execution.</param>
            [Pure]
            public static MethodInfo DeserializerInfo(Type objectType, bool ignoreInheritedAbstract = false) => GetMethodInfo(objectType, nameof(TypeProcessor<object>.DeserializerInfo), ignoreInheritedAbstract);
        }
    }
}
