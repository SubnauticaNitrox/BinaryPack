using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;
using BinaryPack.Models.Interfaces;
using MessagePack;
using MessagePack.Resolvers;
using Portable.Xaml;
using Utf8Json;

namespace BinaryPack.Comparison.Implementation
{
    /// <summary>
    /// A file size comparer for a generic type using different serialization libraries
    /// </summary>
    /// <typeparam name="T">The type of model to serialize</typeparam>
    internal static class FileSizeComparer
    {
        public static void Run<T>() where T : class, IInitializable, new()
        {
            T model = new T();
            model.Initialize();

            // JSON
            var json = CalculateFileSize(stream => JsonSerializer.Serialize(stream, model));

            // XML
            var xml = CalculateFileSize(stream =>
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stream, model);
            });

            // XAML
            var xaml = CalculateFileSize(stream => XamlServices.Save(stream, model));

            // MessagePack
            var messagePack = CalculateFileSize(stream => MessagePackSerializer.Serialize(stream, model, ContractlessStandardResolver.Options));

#if NETFRAMEWORK
            // BinaryPack
            var binaryPack = CalculateFileSize(stream => BinaryConverter.Serialize(model, stream));
#endif

            // Report
            StringBuilder builder = new ();
            builder.AppendLine("=================");
            builder.AppendLine("Default output");
            builder.AppendLine("GZip output (CompressionLevel = Fastest)");
            builder.AppendLine("GZip output (CompressionLevel = Optimal)");
            builder.AppendLine("=================");
            builder.AppendLine($"{"JSON",-20}{json.Plain,7}{json.GZipFastest,10}{json.GZipOptimal,10}");
            builder.AppendLine($"{"XML",-20}{xml.Plain,7}{xml.GZipFastest,10}{xml.GZipOptimal,10}");
            builder.AppendLine($"{"XAML",-20}{xaml.Plain,7}{xaml.GZipFastest,10}{xaml.GZipOptimal,10}");
            builder.AppendLine($"{"MessagePack",-20}{messagePack.Plain,7}{messagePack.GZipFastest,10}{messagePack.GZipOptimal,10}");
#if NETFRAMEWORK
            builder.AppendLine($"{"BinaryPack (Nitrox)",-20}{binaryPack.Plain,7}{binaryPack.GZipFastest,10}{binaryPack.GZipOptimal,10}");
#endif
            Console.WriteLine(builder);
        }

        /// <summary>
        /// Calculates the file size for a given serializer
        /// </summary>
        /// <param name="f">An <see cref="Action{T}"/> that writes the serialized data to a given <see cref="Stream"/></param>
        [Pure]
        private static (long Plain, long GZipFastest, long GZipOptimal) CalculateFileSize(Action<Stream> f)
        {
            using MemoryStream stream = new ();
            f(stream);

            long plain = stream.Position;

            using MemoryStream outputFastest = new ();
            using GZipStream gzipFastest = new (outputFastest, CompressionLevel.Fastest);

            stream.Seek(0, SeekOrigin.Begin);
            stream.CopyTo(gzipFastest);

            using MemoryStream outputOptimal = new ();
            using GZipStream gzipOptimal = new (outputOptimal, CompressionLevel.Optimal);

            stream.Seek(0, SeekOrigin.Begin);
            stream.CopyTo(gzipOptimal);

            return (plain, outputFastest.Position, outputOptimal.Position);
        }
    }
}
