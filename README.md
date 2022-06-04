!["Nitrox Subnautica Multiplayer Mod"](https://i.imgur.com/ofnNX5z.gif)

# Nitrox BinaryPack

[![NuGet](https://img.shields.io/nuget/v/BinaryPack.svg)](https://www.nuget.org/packages/BinaryPack/)
[![Discord](https://img.shields.io/discord/525437013403631617?logo=discord&logoColor=white)](https://discord.gg/E8B4X9s)
[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)

A fork of the official [`BinaryPack`](https://github.com/Sergio0694/BinaryPack/). It has a few additional features and provides support for net472.

> **DISCLAIMER:** The underling library is provided as is, and it's no longer being actively maintained. **BinaryPack** was developed just for fun and it has not been tested in critical production environments. It does work fine in the scenarios described in this document (eg. I'm using this library to handle local cache files in some of my apps), but if you're looking for a more widely used and well tested library for fast binary serialization (that also offers better flexibility and customization), I'd recommend looking into [`MessagePack-CSharp`](https://github.com/neuecc/MessagePack-CSharp) first.

## Supported properties

Here is a list of the property types currently supported by the library:

✅ Primitive types (except `object`): `string`, `bool`, `int`, `uint`, `float`, `double`, etc.

✅ Nullable value types: `Nullable<T>` or `T?` for short, where `T : struct`

✅ Unmanaged types: eg. `System.Numerics.Vector2`, and all `unmanaged` value types

✅ Unions: eg. `abstract` classes

✅ .NET arrays: `T[]`, `T[,]`, `T[,,]`, etc.

✅ .NET collections: `List<T>`, `IList<T>`, `ICollection<T>`, `IEnumerable<T>`, etc.

✅ .NET dictionaries: `Dictionary<TKey, TValue>`, `IDictionary<TKey, TValue>` and `IReadOnlyDictionary<TKey, TValue>`

✅ Other .NET types: `BitArray`

# Requirements

This **BinaryPack** fork requires .NET Framework 4.7.2

The test projects required .NET CoreApp 3.1
