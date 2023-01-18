﻿using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;

namespace System.Reflection.Emit
{
    /// <summary>
    /// A <see langword="class"/> that can be used to easily (lol) create dynamic methods
    /// </summary>
    /// <typeparam name="T">The type of <see cref="Delegate"/> that will be used to wrap the new methods</typeparam>
    internal sealed class DynamicMethod<T> where T : Delegate
    {
        /// <summary>
        /// The owner type for new dynamic methods
        /// </summary>
        private static readonly Type OwnerType = typeof(DynamicMethod<T>);

        /// <summary>
        /// The return type of the <typeparamref name="T"/> <see langword="delegate"/>
        /// </summary>
        private static readonly Type ReturnType;

        /// <summary>
        /// The types of the arguments of the <typeparamref name="T"/> <see langword="delegate"/>
        /// </summary>
        private static readonly Type[] ParameterTypes;

        /// <summary>
        /// Loads all the necessary <see cref="Type"/> info for the current <typeparamref name="T"/> parameter
        /// </summary>
        static DynamicMethod()
        {
            MethodInfo method = typeof(T).GetMethod("Invoke");
            ReturnType = method.ReturnType;
            ParameterTypes = method.GetParameters().Select(p => p.ParameterType).ToArray();
        }

        /// <summary>
        /// Creates a new <see cref="DynamicMethod{T}"/> instance wrapping a given <see cref="DynamicMethod"/>
        /// </summary>
        /// <param name="dynamicMethod">The new <see cref="DynamicMethod"/> instance currently being built</param>
        private DynamicMethod(DynamicMethod dynamicMethod) => MethodInfo = dynamicMethod;

        /// <summary>
        /// Gets the <see cref="MethodInfo"/> instance representing the new dynamic method being built
        /// </summary>
        public MethodInfo MethodInfo { get; }

        /// <summary>
        /// Local counter for dynamic methods of type <typeparamref name="T"/> ever generated
        /// </summary>
        private static int _Count;

        /// <summary>
        /// Creates a new unique id for a dynamic method
        /// </summary>
        [Pure]
        private static string GetNewId() => $"__IL__{typeof(T).Name}_{typeof(T).GetGenericArguments()[0].Name}_{Interlocked.Increment(ref _Count)}";

        /// <summary>
        /// Creates a new <see cref="DynamicMethod{T}"/> instance with an empty method
        /// </summary>
        /// <returns>A <see cref="DynamicMethod{T}"/> instance wrapping an empty <see cref="DynamicMethod"/></returns>
        [Pure]
        public static DynamicMethod<T> New()
        {
            // Create a new dynamic method
            DynamicMethod method = new(GetNewId(), ReturnType, ParameterTypes, OwnerType, true);

            return new DynamicMethod<T>(method);
        }

        /// <summary>
        /// State tracked to ensure correct order of actions when building
        /// </summary>
        private State state;

        /// <summary>
        /// Emits the IL bytecode to the method
        /// </summary>
        /// <param name="builder">An <see cref="Action"/> that builds the IL bytecode for the new method</param>
        [Pure]
        public void Emit(Action<ILGenerator> builder)
        {
            if (state >= State.Emitted) throw new InvalidOperationException("Emit may only be called once");

            DynamicMethod method = (DynamicMethod)MethodInfo;
            ILGenerator il = method.GetILGenerator();
            builder(il);

            state = State.Emitted;
        }

        /// <summary>
        /// Creates a new <typeparamref name="T"/> <see langword="delegate"/> for the current <see cref="MethodInfo"/> handle
        /// </summary>
        /// <returns>A new dynamic method wrapped as a <typeparamref name="T"/> <see langword="delegate"/></returns>
        [Pure]
        public T Build()
        {
            if (state < State.Emitted) throw new InvalidOperationException("Emit has not been called");

            state = State.Built;

            return (T)MethodInfo.CreateDelegate(typeof(T));
        }

        /// <summary>
        /// The methods that have been called on this object
        /// </summary>
        private enum State
        {
            Initialized,
            Emitted,
            Built
        }
    }
}
