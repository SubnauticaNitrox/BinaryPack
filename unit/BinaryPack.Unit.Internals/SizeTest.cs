using System.IO;
using BinaryPack.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinaryPack.Unit.Internals
{
    [TestClass]
    public class SizeTest
    {
        // Test method for ensuring size of deserialized objects
        private static void Test(int size)
        {
            // Initialize
            DynamicSizeModel model = new DynamicSizeModel();
            model.Initialize(size);

            // Serialize
            using MemoryStream stream = new MemoryStream();
            BinaryConverter.Serialize(model, stream);

            // Deserialize
            stream.Seek(0, SeekOrigin.Begin);
            DynamicSizeModel result = BinaryConverter.Deserialize<DynamicSizeModel>(stream);

            Assert.IsTrue(model.Equals(result));
            Assert.AreEqual(size, stream.Length - 9); //9 bytes are serialization overhead
        }

        [TestMethod]
        public void ZeroDataSerializationTest() => Test(0);

        [TestMethod]
        public void Size1024SerializationTest() => Test(1024);

        [TestMethod]
        public void Size9999SerializationTest() => Test(9999);
    }
}
