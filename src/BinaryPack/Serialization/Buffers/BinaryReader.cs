﻿using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace BinaryPack.Serialization.Buffers
{
    /// <summary>
    /// A <see langword="struct"/> that provides a fast implementation of a binary reader, pulling data from a given <see cref="Span{T}"/>
    /// </summary>
    internal ref struct BinaryReader
    {
        /// <summary>
        /// The <see cref="Span{T}"/> current in use
        /// </summary>
        private readonly ReadOnlySpan<byte> Buffer;

        /// <summary>
        /// The current position into <see cref="Buffer"/>
        /// </summary>
        private int _Position;

        /// <summary>
        /// Creates a new <see cref="BinaryReader"/> instance with the given parameters
        /// </summary>
        /// <param name="buffer">The source<see cref="Span{T}"/> to read data from</param>
        public BinaryReader(ReadOnlySpan<byte> buffer)
        {
            Buffer = buffer;
            _Position = 0;
        }

        /// <summary>
        /// Reads a value of type <typeparamref name="T"/> from the underlying buffer
        /// </summary>
        /// <typeparam name="T">The type of value to read from the buffer</typeparam>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Read<T>() where T : unmanaged
        {
            /* The reasons for declaring this struct as a ref struct, and for
             * carrying a Span<byte> instead of a byte[] array are that this way
             * the reader can also read data from memory areas that are now owned
             * by the caller, or data that is just a slice on another array.
             * These variable declarations are just for clarity, they are
             * all optimized away bit the JIT compiler anyway. */
            ref byte r0 = ref Unsafe.AsRef(Buffer[_Position]);
            T value = Unsafe.As<byte, T>(ref r0);
            _Position += Unsafe.SizeOf<T>();

            return value;
        }

        /// <summary>
        /// Reads a sequence of elements of type <typeparamref name="T"/> from the underlying buffer
        /// </summary>
        /// <typeparam name="T">The type of elements to read from the buffer</typeparam>
        /// <param name="span">The target <see cref="Span{T}"/> to write the read elements to</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Read<T>(Span<T> span) where T : unmanaged
        {
            int size = Unsafe.SizeOf<T>() * span.Length;
            Span<byte> destination = MemoryMarshal.Cast<T, byte>(span);
            ReadOnlySpan<byte> source = Buffer.Slice(_Position, size);

            source.CopyTo(destination);
            _Position += size;
        }
    }
}
