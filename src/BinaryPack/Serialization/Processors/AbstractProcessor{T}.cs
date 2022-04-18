using BinaryPack.Serialization.Constants;
using BinaryPack.Serialization.Processors.Abstract;
using BinaryPack.Serialization.Reflection;
using System;
using System.Linq;
using System.Reflection.Emit;

namespace BinaryPack.Serialization.Processors;

/// <summary>
/// A <see langword="class"/> responsible for creating the serializers and deserializers for <see langword="abstract"/> types
/// </summary>
/// <typeparam name="TBase">The <see langword="abstract"/> type of items to handle during serialization and deserialization</typeparam>
internal sealed partial class AbstractProcessor<TBase> : TypeProcessor<TBase>
{
    /// <summary>
    /// Static <see cref="AbstractProcessor{T}"/> constructor to programmatically validate <typeparamref name="TBase"/>
    /// </summary>
    static AbstractProcessor()
    {
        if (typeof(TBase).IsAbstract || typeof(TBase).IsInterface) return;

        throw new ArgumentException($"{nameof(AbstractProcessor<TBase>)} only works on abstract classes or interfaces, not on [{typeof(TBase)}]");
    }

    private static Type[] unionTypes = Type.EmptyTypes;

    /// <summary>
    /// The subclasses that may be assigned to a member of the <see langword="abstract"/> <see langword="class"/> or <see langword="interface"/>
    /// </summary>
    public static Type[] UnionTypes
    {
        set
        {
            if (unionTypes != Type.EmptyTypes && unionTypes != value)
            {
                throw new InvalidOperationException("Union types can only be defined once");
            }

            unionTypes = value;
        }
    }

    // TODO: Defer call to constructor to avoid race condition
    /// <summary>
    /// Gets the singleton <see cref="AbstractProcessor{T}"/> instance to use
    /// </summary>
    public static AbstractProcessor<TBase> Instance { get; } = new AbstractProcessor<TBase>();

    /// <inheritdoc/>
    protected override void EmitSerializer(ILGenerator il)
    {
        for (int i = 0; i < unionTypes.Length; i++)
        {
            Type sub = unionTypes[i];
            Label isNotInstance = il.DefineLabel();

            // if (obj is TSub) { }
            il.EmitLoadArgument(Arguments.Write.T);
            il.Emit(OpCodes.Isinst, sub);
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Ceq);
            il.Emit(OpCodes.Brtrue_S, isNotInstance);

            // writer.Write(i);
            il.EmitLoadArgument(Arguments.Write.RefBinaryWriter);
            il.EmitLoadInt32(i);
            il.EmitCall(KnownMembers.BinaryWriter.WriteT(typeof(int)));

            // Find and invoke correct serializer for type TSub (similar to ObjectProcessor)
            il.EmitLoadArgument(Arguments.Write.T);
            il.EmitLoadArgument(Arguments.Write.RefBinaryWriter);
            il.EmitCall(KnownMembers.TypeProcessor.SerializerInfo(sub));

            // return;
            il.Emit(OpCodes.Ret);

            il.MarkLabel(isNotInstance);
        }

        // throw new ArgumentException($"Type {obj.GetType()} is not registered in the union for {typeof(TBase)}");
        il.Emit(OpCodes.Ldstr, $"Type {{0}} is not registered in the union for {typeof(TBase)}");
        il.EmitLoadArgument(Arguments.Write.T);
        il.EmitCall(typeof(object).GetMethod(nameof(object.GetType)));
        il.EmitCall(typeof(string).GetMethod(nameof(string.Format), Enumerable.Repeat(typeof(string), 2).ToArray()));
        il.Emit(OpCodes.Newobj, typeof(ArgumentException).GetConstructor(new Type[] { typeof(string) }));
        il.Emit(OpCodes.Throw);
    }

    /// <inheritdoc/>
    protected override void EmitDeserializer(ILGenerator il)
    {
        il.DeclareLocal(typeof(int));

        // int index = reader.Read<int>();
        il.EmitLoadArgument(Arguments.Read.RefBinaryReader);
        il.EmitCall(KnownMembers.BinaryReader.ReadT(typeof(int)));
        il.EmitStoreLocal(Locals.Read.UnionIndex);

        // We cannot simply do unionTypes[index] because the value of index is not known until the method has been invoked
        for (int i = 0; i < unionTypes.Length; i++)
        {
            Type sub = unionTypes[i];
            Label noMatch = il.DefineLabel();

            // if (index == i) { }
            il.EmitLoadLocal(Locals.Read.UnionIndex);
            il.EmitLoadInt32(i);
            il.Emit(OpCodes.Ceq);
            il.Emit(OpCodes.Brfalse_S, noMatch);

            // Find and invoke correct deserializer for type TSub
            il.EmitLoadArgument(Arguments.Read.RefBinaryReader);
            il.EmitCall(KnownMembers.TypeProcessor.DeserializerInfo(sub));

            // return;
            il.Emit(OpCodes.Ret);

            il.MarkLabel(noMatch);
        }

        // throw new IndexOutOfRangeException($"Index {index} is outside the bounds of the union types array for {typeof(TBase)}");
        il.Emit(OpCodes.Ldstr, $"Index {{0}} is outside the bounds of the union types array for {typeof(TBase)}");
        il.EmitLoadLocal(Locals.Read.UnionIndex);
        il.EmitCall(typeof(string).GetMethod(nameof(string.Format), Enumerable.Repeat(typeof(string), 2).ToArray()));
        il.Emit(OpCodes.Newobj, typeof(IndexOutOfRangeException).GetConstructor(new Type[] { typeof(string) }));
        il.Emit(OpCodes.Throw);
    }
}
