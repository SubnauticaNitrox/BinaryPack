using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BinaryPack.Models;
using BinaryPack.Models.Helpers;
using BinaryPack.Serialization.Buffers;
using BinaryPack.Serialization.Processors.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinaryPack.Unit.Internals
{
    [TestClass]
    public class ListTest
    {
        // Test method for a generic list of reference types
        public static void Test<T>(List<T>? list) where T : class, IEquatable<T>
        {
            // Serialization
            BinaryWriter writer = new BinaryWriter(BinaryWriter.DefaultSize);
            ListProcessor<T>.Instance.Serializer(list, ref writer);
            Span<byte> span = MemoryMarshal.CreateSpan(ref Unsafe.AsRef(writer.Span.GetPinnableReference()), writer.Span.Length);
            BinaryReader reader = new BinaryReader(span);
            List<T>? result = ListProcessor<T>.Instance.Deserializer(ref reader);

            // Equality check
            Assert.IsTrue(StructuralComparer.IsMatch(list, result));
        }

        [TestMethod]
        public void ReferenceTypeNullListSerializationTest() => Test(default(List<MessagePackSampleModel>));

        [TestMethod]
        public void ReferenceTypeEmptyListSerializationTest() => Test(new List<MessagePackSampleModel>());

        [TestMethod]
        public void ReferenceTypeListSerializationTest1() => Test(new List<MessagePackSampleModel> { new MessagePackSampleModel { Compact = true, Schema = 17 } });

        [TestMethod]
        public void ReferenceTypeListSerializationTest2() => Test((
            from i in Enumerable.Range(0, 10)
            let compact = i % 2 == 0
            let model = new MessagePackSampleModel { Compact = compact, Schema = i }
            select model).ToList());

        [TestMethod]
        public void ReferenceTypeListSerializationTest3() => Test((
            from i in Enumerable.Range(0, 10)
            let compact = i % 2 == 0
            let model = compact ? null : new MessagePackSampleModel { Compact = compact, Schema = i }
            select model).ToList());

        [TestMethod]
        public void StringNullListSerializationTest() => Test(default(List<string>));

        [TestMethod]
        public void StringEmptyListSerializationTest() => Test(new List<string>());

        [TestMethod]
        public void StringListSerializationTest1() => Test(new List<string> { RandomProvider.NextString(60) });

        [TestMethod]
        public void StringListSerializationTest2() => Test((
            from _ in Enumerable.Range(0, 10)
            select RandomProvider.NextString(60)).ToList());

        [TestMethod]
        public void StringListSerializationTest3() => Test((
            from i in Enumerable.Range(0, 10)
            let isNull = i % 2 == 0
            let text = isNull ? null : RandomProvider.NextString(60)
            select text).ToList());

        // Test method for list of an unmanaged type
        public static void Test(List<DateTime>? list)
        {
            // Serialization
            BinaryWriter writer = new BinaryWriter(BinaryWriter.DefaultSize);
            ListProcessor<DateTime>.Instance.Serializer(list, ref writer);
            Span<byte> span = MemoryMarshal.CreateSpan(ref Unsafe.AsRef(writer.Span.GetPinnableReference()), writer.Span.Length);
            BinaryReader reader = new BinaryReader(span);
            List<DateTime>? result = ListProcessor<DateTime>.Instance.Deserializer(ref reader);

            // Equality check
            Assert.IsTrue(StructuralComparer.IsMatch(list, result));
        }

        [TestMethod]
        public void UnmanagedTypeNullListSerializationTest() => Test(default);

        [TestMethod]
        public void UnmanagedTypeEmptyListSerializationTest() => Test(new List<DateTime>());

        [TestMethod]
        public void UnmanagedTypeListSerializationTest1() => Test(new List<DateTime> { RandomProvider.NextDateTime() });

        [TestMethod]
        public void UnmanagedTypeListSerializationTest2() => Test((
            from i in Enumerable.Range(0, 10)
            select RandomProvider.NextDateTime()).ToList());
    }
}
