``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1826 (21H2)
Intel Core i7-4790K CPU 4.00GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.202
  [Host]  : .NET Core 3.1.22 (CoreCLR 4.700.21.56803, CoreFX 4.700.21.57101), X64 RyuJIT
  Core_31 : .NET Core 3.1.22 (CoreCLR 4.700.21.56803, CoreFX 4.700.21.57101), X64 RyuJIT
  NET_472 : .NET Framework 4.8 (4.8.4515.0), X64 RyuJIT


```
|           Method |     Job |              Runtime |      Categories |        Mean |     Error |    StdDev | Ratio | RatioSD |    Gen 0 |   Gen 1 |   Gen 2 | Allocated |
|----------------- |-------- |--------------------- |---------------- |------------:|----------:|----------:|------:|--------:|---------:|--------:|--------:|----------:|
|   NewtonsoftJson | Core_31 |        .NET Core 3.1 | Deserialization |   730.72 μs |  6.163 μs |  5.764 μs |  1.00 |    0.00 |  39.0625 |  8.7891 |       - |    160 KB |
|  BinaryFormatter | Core_31 |        .NET Core 3.1 | Deserialization |   564.47 μs |  7.138 μs |  6.677 μs |  0.77 |    0.01 |  79.1016 | 25.3906 |       - |    354 KB |
|      NetCoreJson | Core_31 |        .NET Core 3.1 | Deserialization |   680.27 μs |  4.891 μs |  4.575 μs |  0.93 |    0.01 |  26.3672 |  2.9297 |       - |    109 KB |
|    XmlSerializer | Core_31 |        .NET Core 3.1 | Deserialization |   748.63 μs |  8.086 μs |  6.752 μs |  1.03 |    0.02 |  44.9219 |       - |       - |    186 KB |
| DataContractJson | Core_31 |        .NET Core 3.1 | Deserialization | 2,658.74 μs | 24.954 μs | 23.342 μs |  3.64 |    0.04 | 140.6250 | 35.1563 |       - |    578 KB |
|         Utf8Json | Core_31 |        .NET Core 3.1 | Deserialization |   404.14 μs |  5.773 μs |  5.400 μs |  0.55 |    0.01 |  41.5039 | 41.5039 | 41.5039 |    234 KB |
|      MessagePack | Core_31 |        .NET Core 3.1 | Deserialization |   128.44 μs |  1.818 μs |  1.612 μs |  0.18 |    0.00 |  23.4375 |  5.6152 |       - |     97 KB |
|       BinaryPack | Core_31 |        .NET Core 3.1 | Deserialization |    54.56 μs |  0.514 μs |  0.480 μs |  0.07 |    0.00 |  22.2778 |  0.1831 |       - |     91 KB |
|                  |         |                      |                 |             |           |           |       |         |          |         |         |           |
|   NewtonsoftJson | Core_31 |        .NET Core 3.1 |   Serialization |   583.42 μs |  6.262 μs |  5.551 μs |  1.00 |    0.00 |  41.0156 | 41.0156 | 41.0156 |    295 KB |
|  BinaryFormatter | Core_31 |        .NET Core 3.1 |   Serialization |   513.58 μs |  4.932 μs |  4.372 μs |  0.88 |    0.01 |  62.5000 | 15.6250 |       - |    256 KB |
|      NetCoreJson | Core_31 |        .NET Core 3.1 |   Serialization |   484.44 μs |  4.756 μs |  4.216 μs |  0.83 |    0.01 |  76.1719 | 75.1953 | 38.0859 |    299 KB |
|    XmlSerializer | Core_31 |        .NET Core 3.1 |   Serialization |   688.90 μs |  9.934 μs |  9.293 μs |  1.18 |    0.02 | 102.5391 | 60.5469 | 41.0156 |    477 KB |
| DataContractJson | Core_31 |        .NET Core 3.1 |   Serialization |   847.21 μs | 16.446 μs | 23.054 μs |  1.43 |    0.04 |  34.1797 | 34.1797 | 34.1797 |    226 KB |
|         Utf8Json | Core_31 |        .NET Core 3.1 |   Serialization |   387.30 μs |  7.543 μs | 10.575 μs |  0.66 |    0.02 |  41.5039 | 41.5039 | 41.5039 |    196 KB |
|      MessagePack | Core_31 |        .NET Core 3.1 |   Serialization |    81.34 μs |  1.617 μs |  2.565 μs |  0.14 |    0.00 |  25.8789 |  5.1270 |       - |    107 KB |
|       BinaryPack | Core_31 |        .NET Core 3.1 |   Serialization |    59.50 μs |  1.148 μs |  1.322 μs |  0.10 |    0.00 |  11.1084 |  1.3428 |       - |     46 KB |
|                  |         |                      |                 |             |           |           |       |         |          |         |         |           |
|   NewtonsoftJson | NET_472 | .NET Framework 4.7.2 | Deserialization |   833.02 μs | 16.206 μs | 18.662 μs |  1.00 |    0.00 |  41.0156 |       - |       - |    171 KB |
|  BinaryFormatter | NET_472 | .NET Framework 4.7.2 | Deserialization |   722.38 μs | 13.760 μs | 15.295 μs |  0.87 |    0.03 |  90.8203 | 28.3203 |       - |    373 KB |
|      NetCoreJson | NET_472 | .NET Framework 4.7.2 | Deserialization | 1,018.29 μs | 19.782 μs | 19.429 μs |  1.22 |    0.03 |  25.3906 |  5.8594 |       - |    109 KB |
|    XmlSerializer | NET_472 | .NET Framework 4.7.2 | Deserialization |   939.82 μs | 17.311 μs | 17.002 μs |  1.13 |    0.04 |  48.8281 | 11.7188 |       - |    203 KB |
| DataContractJson | NET_472 | .NET Framework 4.7.2 | Deserialization | 2,792.53 μs | 53.890 μs | 52.927 μs |  3.35 |    0.10 | 140.6250 |       - |       - |    588 KB |
|         Utf8Json | NET_472 | .NET Framework 4.7.2 | Deserialization |   507.56 μs | 10.007 μs | 13.697 μs |  0.61 |    0.02 |  41.0156 | 41.0156 | 41.0156 |    243 KB |
|      MessagePack | NET_472 | .NET Framework 4.7.2 | Deserialization |   283.43 μs |  3.039 μs |  2.842 μs |  0.34 |    0.01 |  26.8555 |  4.3945 |       - |    111 KB |
|       BinaryPack | NET_472 | .NET Framework 4.7.2 | Deserialization |   131.14 μs |  2.096 μs |  1.961 μs |  0.16 |    0.00 |  45.4102 |  9.0332 |       - |    189 KB |
|                  |         |                      |                 |             |           |           |       |         |          |         |         |           |
|   NewtonsoftJson | NET_472 | .NET Framework 4.7.2 |   Serialization |   715.58 μs |  9.049 μs |  8.464 μs |  1.00 |    0.00 |  41.0156 | 41.0156 | 41.0156 |    296 KB |
|  BinaryFormatter | NET_472 | .NET Framework 4.7.2 |   Serialization |   708.37 μs |  8.516 μs |  7.966 μs |  0.99 |    0.02 |  76.1719 | 21.4844 |       - |    314 KB |
|      NetCoreJson | NET_472 | .NET Framework 4.7.2 |   Serialization |   850.44 μs | 10.149 μs |  9.493 μs |  1.19 |    0.02 |  66.4063 | 66.4063 | 66.4063 |    334 KB |
|    XmlSerializer | NET_472 | .NET Framework 4.7.2 |   Serialization |   921.63 μs |  9.804 μs |  9.170 μs |  1.29 |    0.02 | 124.0234 | 82.0313 | 41.0156 |    490 KB |
| DataContractJson | NET_472 | .NET Framework 4.7.2 |   Serialization |   906.79 μs |  8.220 μs |  7.287 μs |  1.27 |    0.02 |  35.1563 | 35.1563 | 35.1563 |    229 KB |
|         Utf8Json | NET_472 | .NET Framework 4.7.2 |   Serialization |   416.66 μs |  5.498 μs |  5.143 μs |  0.58 |    0.01 |  41.5039 | 41.5039 | 41.5039 |    201 KB |
|      MessagePack | NET_472 | .NET Framework 4.7.2 |   Serialization |   132.01 μs |  2.336 μs |  2.185 μs |  0.18 |    0.00 |  26.1230 |  4.1504 |       - |    108 KB |
|       BinaryPack | NET_472 | .NET Framework 4.7.2 |   Serialization |   149.00 μs |  1.835 μs |  1.716 μs |  0.21 |    0.00 |  15.3809 |  2.4414 |       - |     64 KB |
