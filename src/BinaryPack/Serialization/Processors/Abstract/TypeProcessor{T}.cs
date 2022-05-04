using System.Reflection.Emit;
using BinaryPack.Delegates;

namespace BinaryPack.Serialization.Processors.Abstract
{
    /// <summary>
    /// A <see langword="class"/> responsible for creating the serializers and deserializers for a given type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">The type of items to serialize and deserialize</typeparam>
    internal abstract class TypeProcessor<T>
    {
        private readonly DynamicMethod<BinarySerializer<T>> serializerInfo = DynamicMethod<BinarySerializer<T>>.New();

        /// <summary>
        /// The <see cref="DynamicMethod{T}"/> instance holding the serializer being built for items of type <typeparamref name="T"/>
        /// </summary>
        public DynamicMethod<BinarySerializer<T>> SerializerInfo
        {
            get
            {
                if (serializer == null)
                {
                    BuildMethods();
                }

                return serializerInfo;
            }
        }

        private readonly DynamicMethod<BinaryDeserializer<T>> deserializerInfo = DynamicMethod<BinaryDeserializer<T>>.New();

        /// <summary>
        /// The <see cref="DynamicMethod{T}"/> instance holding the deserializer being built for items of type <typeparamref name="T"/>
        /// </summary>
        public DynamicMethod<BinaryDeserializer<T>> DeserializerInfo
        {
            get
            {
                if (deserializer == null)
                {
                    BuildMethods();
                }

                return deserializerInfo;
            }
        }

        private BinarySerializer<T>? serializer;

        /// <summary>
        /// Gets the <see cref="BinarySerializer{T}"/> instance for items of the current type <typeparamref name="T"/>
        /// </summary>
        public BinarySerializer<T> Serializer
        {
            get
            {
                if (serializer == null)
                {
                    BuildMethods();
                }

                return serializer!;
            }
        }

        private BinaryDeserializer<T>? deserializer;

        /// <summary>
        /// Gets the <see cref="BinaryDeserializer{T}"/> instance for items of the current type <typeparamref name="T"/>
        /// </summary>
        public BinaryDeserializer<T> Deserializer
        {
            get
            {
                if (deserializer == null)
                {
                    BuildMethods();
                }

                return deserializer!;
            }
        }

        private bool building;

        private void BuildMethods()
        {
            if (building)
            {
                return;
            }

            building = true;

            serializer = SerializerInfo.Build(EmitSerializer);
            deserializer = DeserializerInfo.Build(EmitDeserializer);

            building = false;
        }

        /// <summary>
        /// Emits the instructions for the dynamic serializer method for items of type <typeparamref name="T"/>
        /// </summary>
        /// <param name="il">The input <see cref="ILGenerator"/> instance to use to emit instructions</param>
        protected abstract void EmitSerializer(ILGenerator il);

        /// <summary>
        /// Emits the instructions for the dynamic deserializer method for items of type <typeparamref name="T"/>
        /// </summary>
        /// <param name="il">The input <see cref="ILGenerator"/> instance to use to emit instructions</param>
        protected abstract void EmitDeserializer(ILGenerator il);
    }
}
