using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using BinaryPack.Attributes;
using BinaryPack.Enums;

namespace System
{
    /// <summary>
    /// A <see langword="class"/> that provides extension methods for the <see cref="Type"/> type
    /// </summary>
    internal static class TypeExtensions
    {
        /// <summary>
        /// Enumerates all the members of the input <see cref="Type"/> that are supported for serialization
        /// </summary>
        /// <param name="type">The input type to analyze</param>
        [Pure]
        public static IReadOnlyCollection<MemberInfo> GetSerializableMembers(this Type type)
        {
            BinarySerializationAttribute attribute = type.GetCustomAttribute<BinarySerializationAttribute>();
            SerializationMode mode = attribute?.Mode ?? SerializationMode.AllMembers;
            IReadOnlyCollection<MemberInfo> members = (mode switch
            {
                /* If the mode is set to explicit, we query all the members of the
                 * target type, both public and not public, and return the ones that
                 * would respect either the Properties or Fields modes, and that
                 * are explicitly marked as serializable members. */
                SerializationMode.Explicit =>

                from member in type.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                where (member.MemberType == MemberTypes.Property ||
                       member.MemberType == MemberTypes.Field) &&
                      member.IsDefined(typeof(SerializableMemberAttribute)) &&
                      (member is FieldInfo fieldInfo && !fieldInfo.IsInitOnly && !fieldInfo.IsLiteral ||
                       member is PropertyInfo propertyInfo && propertyInfo.CanRead
                       && propertyInfo.GetIndexParameters().Length == 0)
                select member,

                /* For all other modes, we first retrieve all the candidate members from the
                 * input type - that is, public instance members by default, plus non public members
                 * too if the PublicMembersOnly flag is not selected. Then we filter out either
                 * properties or fields (or neither of them) as requested, skip the members
                 * that are marked as non serializable and finally select the available members as before. */
                _ =>

                from member in type.GetMembers(
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                where (mode.HasFlag(SerializationMode.Properties) && member.MemberType == MemberTypes.Property ||
                       mode.HasFlag(SerializationMode.Fields) && member.MemberType == MemberTypes.Field) &&
                      !member.IsDefined(typeof(IgnoredMemberAttribute)) &&
                      (mode.HasFlag(SerializationMode.NonPublicMembers) || member.IsPublic() ||
                       member.IsDefined(typeof(SerializableMemberAttribute))) &&
                      (member is FieldInfo fieldInfo && !fieldInfo.IsInitOnly && !fieldInfo.IsLiteral ||
                       member is PropertyInfo propertyInfo && propertyInfo.CanRead
                       && propertyInfo.GetMethod.GetCustomAttribute<CompilerGeneratedAttribute>() != null
                       && propertyInfo.GetIndexParameters().Length == 0)
                select member
            }).SortMemberAsConstructorParameters(type)
              .ToArray();

            return members;
        }


        /// Finding a constructor where all serialized members have a parameter with same type and same name (ignoring case)
        private static IEnumerable<MemberInfo> SortMemberAsConstructorParameters(this IEnumerable<MemberInfo> members, Type type)
        {
            // Checking for constructor with no parameter and even any constructors first
            ConstructorInfo emptyConstructor = type.GetConstructor(Type.EmptyTypes);
            if ((emptyConstructor != null && !emptyConstructor.IsDefined(typeof(IgnoreConstructorAttribute))) ||
                type.GetConstructors().Length == 0)
            {
                // Filter out properties without setter
                return members.Where(member => member is not PropertyInfo propertyInfo || propertyInfo.CanWrite).OrderBy(member => member.Name);
            }

            // Caching to prevent multiple iterations.
            IEnumerable<MemberInfo> memberInfos = members as MemberInfo[] ?? members.ToArray();

            List<MemberInfo> orderedMembers = new(memberInfos.Count());

            foreach (ConstructorInfo constructor in type.GetConstructors())
            {
                if (constructor.IsDefined(typeof(IgnoreConstructorAttribute)))
                {
                    continue;
                }

                if (constructor.GetParameters().Length != memberInfos.Count() &&
                    !constructor.IsDefined(typeof(ForceUseConstructorAttribute)))
                {
                    continue;
                }

                orderedMembers.Clear();
                bool foundAllParameters = true;

                foreach (ParameterInfo parameter in constructor.GetParameters())
                {
                    MemberInfo? memberInfoForParameter = memberInfos.FirstOrDefault(x => x.GetMemberType() == parameter.ParameterType &&
                                                                                         string.Equals(x.Name, parameter.Name, StringComparison.OrdinalIgnoreCase));

                    if (memberInfoForParameter == null)
                    {
                        foundAllParameters = false;
                        break;
                    }

                    orderedMembers.Add(memberInfoForParameter);
                }

                if (foundAllParameters)
                {
                    return orderedMembers;
                }
            }

            throw new InvalidOperationException($"The type {type} has no constructor with parameters corresponding to all members.");
        }

        /// <summary>
        /// Gets the size in bytes of the given type
        /// </summary>
        /// <param name="type">The input type to analyze</param>
        [Pure]
        public static int GetSize(this Type type) => (int)typeof(Unsafe).GetMethod(nameof(Unsafe.SizeOf)).MakeGenericMethod(type).Invoke(null, null);

        /// <summary>
        /// Checks whether the input type is a <see cref="ValueTuple"/> instance of any kind
        /// </summary>
        /// <param name="type">The input type to analyze</param>
        /// <remarks>Code from <see href="https://www.tabsoverspaces.com/233605-checking-whether-the-type-is-a-tuple-valuetuple"/></remarks>
        /// <returns><see langword="true"/> if <paramref name="type"/> represents a <see cref="ValueTuple"/>, <see langword="false"/> otherwise</returns>
        [Pure]
        public static bool IsValueTuple(this Type type)
        {
            if (!type.IsGenericType) return false;

            Type openType = type.GetGenericTypeDefinition();
            return
                openType == typeof(ValueTuple<>) ||
                openType == typeof(ValueTuple<,>) ||
                openType == typeof(ValueTuple<,,>) ||
                openType == typeof(ValueTuple<,,,>) ||
                openType == typeof(ValueTuple<,,,,>) ||
                openType == typeof(ValueTuple<,,,,,>) ||
                openType == typeof(ValueTuple<,,,,,,>) ||
                openType == typeof(ValueTuple<,,,,,,,>) && type.GetGenericArguments()[7].IsValueTuple();
        }

        /// <summary>
        /// Checks whether or not the input type respects the <see langword="unmanaged"/> constraint
        /// </summary>
        /// <param name="type">The input type to analyze</param>
        /// <remarks>Code partially from <see href="https://stackoverflow.com/questions/53968920"/></remarks>
        /// <returns><see langword="true"/> if <paramref name="type"/> respects the <see langword="unmanaged"/> constraint, <see langword="false"/> otherwise</returns>
        [Pure]
        public static bool IsUnmanaged(this Type type)
        {
            if (!type.IsValueType) return false;
            if (type.IsGenericType && !type.IsValueTuple()) return false;

            return
                type.IsPrimitive ||
                type == typeof(decimal) ||
                type.IsPointer ||
                type.IsEnum ||
                type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).All(f => f.FieldType.IsUnmanaged());
        }

        /// <summary>
        /// Checks whether or not the input type is a generic type that matches a given definition
        /// </summary>
        /// <param name="type">The input type to analyze</param>
        /// <param name="target">The generic type to compare the input type to</param>
        [Pure]
        public static bool IsGenericType(this Type type, Type target) => type.IsGenericType &&
                                                                         type.GetGenericTypeDefinition() == target;

        /// <summary>
        /// Gets a generic instantiation of a nested type for a given generic type
        /// </summary>
        /// <param name="type">The input generic type</param>
        /// <param name="name">The name of the nested <see cref="Type"/> to retrieve</param>
        [Pure]
        public static Type GetGenericNestedType(this Type type, string name)
        {
            Type nestedType = type.GetNestedType(name, BindingFlags.Public | BindingFlags.NonPublic);
            return nestedType.MakeGenericType(type.GenericTypeArguments);
        }

        /// <summary>
        /// Checks whether or not the input type is a single-dimensional array with a lower bound of 0.
        /// </summary>
        /// <param name="type">The input type to analyze</param>
        public static bool IsSZArray(this Type type)
        {
            return type.IsArray && type.GetArrayRank() == 1 &&
                ((Array)Activator.CreateInstance(type, new object[] { 0 }))
                .GetLowerBound(0) == 0;
        }

        /// <summary>
        /// Checks whether or not the input type is a multi-dimensional array with at least one dimension having a non-zero lower bound.
        /// </summary>
        /// <param name="type">The input type to analyze</param>
        public static bool IsVariableBoundArray(this Type type)
        {
            if (!(type.IsArray && type.GetArrayRank() > 1))
            {
                return false;
            }

            Array array = (Array)Activator.CreateInstance(type, Enumerable.Repeat(0, type.GetArrayRank()).Cast<object>().ToArray());

            for (int dimension = 0; dimension < array.Rank; dimension++)
            {
                if (array.GetLowerBound(dimension) != 0)
                {
                    return true;
                }
            }

            return false;
        }

        public static Type? GetAbstractBaseType(this Type type)
        {
            Type? baseType = type;

            while (!baseType.IsAbstract)
            {
                if (baseType.BaseType == null)
                {
                    return null;
                }

                baseType = baseType.BaseType;
            }

            return baseType;
        }
    }
}
