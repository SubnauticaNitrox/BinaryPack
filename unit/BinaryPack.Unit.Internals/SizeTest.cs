using System.IO;
using BinaryPack.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinaryPack.Unit.Internals
{
    [TestClass]
    public class SizeTest
    {
        // Test method for ensuring size of deserialized objects
        [TestMethod]
        [DataRow(0)]
        [DataRow(1024)]
        [DataRow(9999)]
        public void SizeSerializationTest(int size)
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
    }
}
