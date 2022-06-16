!["Nitrox Subnautica Multiplayer Mod"](https://i.imgur.com/ofnNX5z.gif)

# Nitrox BinaryPack

[![NuGet](https://img.shields.io/nuget/v/BinaryPack.svg)](https://www.nuget.org/packages/BinaryPack/)
[![Discord](https://img.shields.io/discord/525437013403631617?logo=discord&logoColor=white)](https://discord.gg/E8B4X9s)
[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)

A fork of the official [`BinaryPack`](https://github.com/Sergio0694/BinaryPack/). It has a few additional features and provides support for net472.

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

## Benchmarks

Here are benchmarks executed with the benchmark sample ([JsonResponseModel](https://github.com/Sergio0694/BinaryPack/blob/master/unit/BinaryPack.Models/JsonResponseModel.cs)) included in this repository.

### Speed Diagnosticts

|              Method |    .NET Runtime |      Categories |         Mean |     Error |    StdDev | Ratio | RatioSD |
|-------------------- |---------------- |---------------- |-------------:|----------:|----------:|------:|--------:|
|      NewtonsoftJson |        Core 3.1 | Deserialization |    655.70 us |  2.860 us |  2.535 us |  1.00 |    0.00 |
|     BinaryFormatter |        Core 3.1 | Deserialization |    542.91 us |  2.744 us |  2.567 us |  0.83 |    0.00 |
|         NetCoreJson |        Core 3.1 | Deserialization |    638.89 us |  0.568 us |  0.444 us |  0.97 |    0.00 |
|       XmlSerializer |        Core 3.1 | Deserialization |    704.78 us |  1.727 us |  1.442 us |  1.07 |    0.01 |
|         PortableXml |        Core 3.1 | Deserialization |  9,920.88 us | 50.469 us | 47.209 us | 15.13 |    0.11 |
|    DataContractJson |        Core 3.1 | Deserialization |  2,339.89 us |  3.195 us |  2.832 us |  3.57 |    0.01 |
|            Utf8Json |        Core 3.1 | Deserialization |    362.29 us |  2.824 us |  2.641 us |  0.55 |    0.00 |
|         MessagePack |        Core 3.1 | Deserialization |    123.50 us |  0.203 us |  0.180 us |  0.19 |    0.00 |
|    BinaryPack (Old) |        Core 3.1 | Deserialization |     50.45 us |  0.130 us |  0.115 us |  0.08 |    0.00 |
|                     |                 |                 |              |           |           |       |         |
|      NewtonsoftJson |        Core 3.1 |   Serialization |    542.60 us |  1.962 us |  1.739 us |  1.00 |    0.00 |
|     BinaryFormatter |        Core 3.1 |   Serialization |    459.62 us |  1.581 us |  1.401 us |  0.85 |    0.00 |
|         NetCoreJson |        Core 3.1 |   Serialization |    555.86 us |  1.982 us |  1.655 us |  1.02 |    0.00 |
|       XmlSerializer |        Core 3.1 |   Serialization |    643.84 us |  2.549 us |  2.129 us |  1.19 |    0.01 |
|         PortableXml |        Core 3.1 |   Serialization |  4,792.20 us | 23.136 us | 21.641 us |  8.83 |    0.05 |
|    DataContractJson |        Core 3.1 |   Serialization |    791.81 us |  2.200 us |  1.837 us |  1.46 |    0.01 |
|            Utf8Json |        Core 3.1 |   Serialization |    339.00 us |  0.541 us |  0.451 us |  0.62 |    0.00 |
|         MessagePack |        Core 3.1 |   Serialization |     75.16 us |  0.162 us |  0.144 us |  0.14 |    0.00 |
|    BinaryPack (Old) |        Core 3.1 |   Serialization |     54.26 us |  0.082 us |  0.077 us |  0.10 |    0.00 |
|                     |                 |                 |              |           |           |       |         |
|      NewtonsoftJson | Framework 4.7.2 | Deserialization |    720.38 us |  1.123 us |  0.996 us |  1.00 |    0.00 |
|     BinaryFormatter | Framework 4.7.2 | Deserialization |    639.72 us |  0.556 us |  0.493 us |  0.89 |    0.00 |
|         NetCoreJson | Framework 4.7.2 | Deserialization |    926.25 us |  0.648 us |  0.574 us |  1.29 |    0.00 |
|       XmlSerializer | Framework 4.7.2 | Deserialization |    786.80 us |  1.147 us |  0.958 us |  1.09 |    0.00 |
|         PortableXml | Framework 4.7.2 | Deserialization | 12,079.89 us | 94.994 us | 88.857 us | 16.77 |    0.13 |
|    DataContractJson | Framework 4.7.2 | Deserialization |  2,572.43 us |  2.759 us |  2.304 us |  3.57 |    0.01 |
|            Utf8Json | Framework 4.7.2 | Deserialization |    457.98 us |  0.539 us |  0.450 us |  0.64 |    0.00 |
|         MessagePack | Framework 4.7.2 | Deserialization |    282.45 us |  0.663 us |  0.554 us |  0.39 |    0.00 |
| BinaryPack (Nitrox) | Framework 4.7.2 | Deserialization |    117.54 us |  0.145 us |  0.121 us |  0.16 |    0.00 |
|                     |                 |                 |              |           |           |       |         |
|      NewtonsoftJson | Framework 4.7.2 |   Serialization |    527.62 us |  1.393 us |  1.235 us |  1.00 |    0.00 |
|     BinaryFormatter | Framework 4.7.2 |   Serialization |    659.28 us |  1.596 us |  1.246 us |  1.25 |    0.00 |
|         NetCoreJson | Framework 4.7.2 |   Serialization |    827.09 us |  0.889 us |  0.788 us |  1.57 |    0.00 |
|       XmlSerializer | Framework 4.7.2 |   Serialization |    809.27 us |  1.399 us |  1.092 us |  1.53 |    0.00 |
|         PortableXml | Framework 4.7.2 |   Serialization |  6,438.72 us |  8.980 us |  8.400 us | 12.20 |    0.03 |
|    DataContractJson | Framework 4.7.2 |   Serialization |    813.18 us |  3.583 us |  3.352 us |  1.54 |    0.01 |
|            Utf8Json | Framework 4.7.2 |   Serialization |    364.15 us |  0.737 us |  0.575 us |  0.69 |    0.00 |
|         MessagePack | Framework 4.7.2 |   Serialization |    114.24 us |  0.128 us |  0.100 us |  0.22 |    0.00 |
| BinaryPack (Nitrox) | Framework 4.7.2 |   Serialization |    123.50 us |  0.242 us |  0.214 us |  0.23 |    0.00 |

### Memory Diagnosticts

|              Method |    .NET Runtime |      Categories |    Gen 0 |   Gen 1 |   Gen 2 | Allocated |
|-------------------- |---------------- |---------------- |---------:|--------:|--------:|----------:|
|      NewtonsoftJson |        Core 3.1 | Deserialization |  39.0625 |  9.7656 |       - |    160 KB |
|     BinaryFormatter |        Core 3.1 | Deserialization |  75.1953 | 23.4375 |       - |    357 KB |
|         NetCoreJson |        Core 3.1 | Deserialization |  26.3672 |  5.8594 |       - |    108 KB |
|       XmlSerializer |        Core 3.1 | Deserialization |  46.8750 |  5.8594 |       - |    195 KB |
|         PortableXml |        Core 3.1 | Deserialization | 265.6250 | 62.5000 |       - |  1,412 KB |
|    DataContractJson |        Core 3.1 | Deserialization | 132.8125 | 23.4375 |       - |    551 KB |
|            Utf8Json |        Core 3.1 | Deserialization |  41.5039 | 41.5039 | 41.5039 |    234 KB |
|         MessagePack |        Core 3.1 | Deserialization |  25.6348 |  0.4883 |       - |    105 KB |
|    BinaryPack (Old) |        Core 3.1 | Deserialization |  23.8037 |  0.3052 |       - |     97 KB |
|                     |                 |                 |          |         |         |           |
|      NewtonsoftJson |        Core 3.1 |   Serialization |  41.0156 | 41.0156 | 41.0156 |    294 KB |
|     BinaryFormatter |        Core 3.1 |   Serialization |  61.5234 |  0.4883 |       - |    254 KB |
|         NetCoreJson |        Core 3.1 |   Serialization |  66.4063 | 66.4063 | 66.4063 |    316 KB |
|       XmlSerializer |        Core 3.1 |   Serialization |  96.6797 | 54.6875 | 41.0156 |    475 KB |
|         PortableXml |        Core 3.1 |   Serialization | 367.1875 | 54.6875 | 23.4375 |  1,612 KB |
|    DataContractJson |        Core 3.1 |   Serialization |  35.1563 | 35.1563 | 35.1563 |    233 KB |
|            Utf8Json |        Core 3.1 |   Serialization |  41.5039 | 41.5039 | 41.5039 |    194 KB |
|         MessagePack |        Core 3.1 |   Serialization |  25.8789 |  5.1270 |       - |    107 KB |
|    BinaryPack (Old) |        Core 3.1 |   Serialization |  11.3525 |  1.4038 |       - |     47 KB |
|                     |                 |                 |          |         |         |           |
|      NewtonsoftJson | Framework 4.7.2 | Deserialization |  37.1094 |  0.9766 |       - |    154 KB |
|     BinaryFormatter | Framework 4.7.2 | Deserialization |  81.0547 | 23.4375 |       - |    366 KB |
|         NetCoreJson | Framework 4.7.2 | Deserialization |  26.3672 |       - |       - |    108 KB |
|       XmlSerializer | Framework 4.7.2 | Deserialization |  46.8750 |  2.9297 |       - |    193 KB |
|         PortableXml | Framework 4.7.2 | Deserialization | 281.2500 | 78.1250 |       - |  1,473 KB |
|    DataContractJson | Framework 4.7.2 | Deserialization | 140.6250 |       - |       - |    580 KB |
|            Utf8Json | Framework 4.7.2 | Deserialization |  41.5039 | 41.5039 | 41.5039 |    250 KB |
|         MessagePack | Framework 4.7.2 | Deserialization |  30.2734 |  1.4648 |       - |    124 KB |
| BinaryPack (Nitrox) | Framework 4.7.2 | Deserialization |  45.4102 |  9.0332 |       - |    188 KB |
|                     |                 |                 |          |         |         |           |
|      NewtonsoftJson | Framework 4.7.2 |   Serialization |  39.0625 |       - |       - |    165 KB |
|     BinaryFormatter | Framework 4.7.2 |   Serialization |  76.1719 | 18.5547 |       - |    316 KB |
|         NetCoreJson | Framework 4.7.2 |   Serialization |  66.4063 | 66.4063 | 66.4063 |    328 KB |
|       XmlSerializer | Framework 4.7.2 |   Serialization |  83.0078 | 41.0156 | 41.0156 |    468 KB |
|         PortableXml | Framework 4.7.2 |   Serialization | 445.3125 | 31.2500 | 23.4375 |  1,909 KB |
|    DataContractJson | Framework 4.7.2 |   Serialization |  34.1797 | 34.1797 | 34.1797 |    227 KB |
|            Utf8Json | Framework 4.7.2 |   Serialization |  41.5039 | 41.5039 | 41.5039 |    196 KB |
|         MessagePack | Framework 4.7.2 |   Serialization |  25.6348 |       - |       - |    106 KB |
| BinaryPack (Nitrox) | Framework 4.7.2 |   Serialization |  15.3809 |       - |       - |     64 KB |

# Requirements

This **BinaryPack** fork requires .NET Framework 4.7.2

The test projects also require .NET CoreApp 3.1
