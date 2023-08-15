``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.3086/22H2/2022Update)
Intel Core i7-4790K CPU 4.00GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
.NET SDK=7.0.304
  [Host]  : .NET 7.0.7 (7.0.723.27404), X64 RyuJIT AVX2
  NET_472 : .NET Framework 4.8.1 (4.8.9166.0), X64 RyuJIT VectorSize=256
  NET_7   : .NET 7.0.7 (7.0.723.27404), X64 RyuJIT AVX2


```
|           Method |     Job |              Runtime |      Categories |        Mean |      Error |     StdDev |      Median | Ratio | RatioSD |     Gen0 |    Gen1 |    Gen2 | Allocated | Alloc Ratio |
|----------------- |-------- |--------------------- |---------------- |------------:|-----------:|-----------:|------------:|------:|--------:|---------:|--------:|--------:|----------:|------------:|
|   NewtonsoftJson | NET_472 | .NET Framework 4.7.2 | Deserialization |   755.44 μs |   8.649 μs |   8.091 μs |   753.70 μs |  1.00 |    0.00 |  39.0625 |  9.7656 |       - | 164.13 KB |        1.00 |
|  BinaryFormatter | NET_472 | .NET Framework 4.7.2 | Deserialization |   602.19 μs |   8.877 μs |   8.304 μs |   601.43 μs |  0.80 |    0.02 |  78.1250 | 25.3906 |       - | 323.74 KB |        1.97 |
|      NetCoreJson | NET_472 | .NET Framework 4.7.2 | Deserialization |   992.52 μs |  15.267 μs |  14.281 μs |   992.06 μs |  1.31 |    0.02 |  27.3438 |  5.8594 |       - | 116.61 KB |        0.71 |
|    XmlSerializer | NET_472 | .NET Framework 4.7.2 | Deserialization |   786.40 μs |   8.094 μs |   6.759 μs |   787.16 μs |  1.04 |    0.01 |  46.8750 |  0.9766 |       - | 192.29 KB |        1.17 |
| DataContractJson | NET_472 | .NET Framework 4.7.2 | Deserialization | 2,360.94 μs |  31.706 μs |  29.658 μs | 2,357.62 μs |  3.13 |    0.06 | 132.8125 | 31.2500 |       - | 558.99 KB |        3.41 |
|         Utf8Json | NET_472 | .NET Framework 4.7.2 | Deserialization |   468.39 μs |   6.252 μs |   5.848 μs |   468.13 μs |  0.62 |    0.01 |  41.5039 | 41.5039 | 41.5039 |  250.7 KB |        1.53 |
|      MessagePack | NET_472 | .NET Framework 4.7.2 | Deserialization |   251.81 μs |   4.508 μs |   4.216 μs |   252.45 μs |  0.33 |    0.01 |  23.9258 |  5.3711 |       - |  99.12 KB |        0.60 |
|       BinaryPack | NET_472 | .NET Framework 4.7.2 | Deserialization |   129.70 μs |   2.287 μs |   2.139 μs |   130.20 μs |  0.17 |    0.00 |  48.5840 | 11.9629 |       - | 200.65 KB |        1.22 |
|                  |         |                      |                 |             |            |            |             |       |         |          |         |         |           |             |
|   NewtonsoftJson |   NET_7 |             .NET 7.0 | Deserialization |   592.42 μs |   6.279 μs |   5.874 μs |   592.28 μs |  1.00 |    0.00 |  40.0391 |  9.7656 |       - |  166.9 KB |        1.00 |
|  BinaryFormatter |   NET_7 |             .NET 7.0 | Deserialization |   444.81 μs |   7.466 μs |   6.234 μs |   443.73 μs |  0.75 |    0.01 |  73.2422 | 21.9727 |       - | 329.58 KB |        1.97 |
|      NetCoreJson |   NET_7 |             .NET 7.0 | Deserialization |   493.91 μs |   6.178 μs |   5.779 μs |   493.08 μs |  0.83 |    0.01 |  27.8320 |       - |       - | 115.35 KB |        0.69 |
|    XmlSerializer |   NET_7 |             .NET 7.0 | Deserialization |   601.00 μs |   8.765 μs |   7.319 μs |   602.15 μs |  1.01 |    0.02 |  44.9219 |  4.8828 |       - | 185.34 KB |        1.11 |
| DataContractJson |   NET_7 |             .NET 7.0 | Deserialization | 2,993.31 μs | 150.925 μs | 445.007 μs | 2,928.61 μs |  6.05 |    0.57 | 148.4375 |  7.8125 |       - |  614.5 KB |        3.68 |
|         Utf8Json |   NET_7 |             .NET 7.0 | Deserialization |   221.03 μs |   4.404 μs |  12.846 μs |   215.98 μs |  0.40 |    0.02 |  23.4375 |  4.3945 |       - |  96.34 KB |        0.58 |
|      MessagePack |   NET_7 |             .NET 7.0 | Deserialization |    90.89 μs |   1.352 μs |   1.265 μs |    91.08 μs |  0.15 |    0.00 |  23.8037 |  2.8076 |       - |  97.49 KB |        0.58 |
|       BinaryPack |   NET_7 |             .NET 7.0 | Deserialization |    47.44 μs |   0.638 μs |   0.596 μs |    47.58 μs |  0.08 |    0.00 |  22.5220 |  4.5166 |       - |     92 KB |        0.55 |
|                  |         |                      |                 |             |            |            |             |       |         |          |         |         |           |             |
|   NewtonsoftJson | NET_472 | .NET Framework 4.7.2 |   Serialization |   635.29 μs |   8.857 μs |   8.285 μs |   636.17 μs |  1.00 |    0.00 |  41.0156 | 41.0156 | 41.0156 | 293.55 KB |        1.00 |
|  BinaryFormatter | NET_472 | .NET Framework 4.7.2 |   Serialization |   642.29 μs |   7.380 μs |   6.162 μs |   643.14 μs |  1.01 |    0.02 |  75.1953 | 18.5547 |       - | 310.19 KB |        1.06 |
|      NetCoreJson | NET_472 | .NET Framework 4.7.2 |   Serialization |   660.66 μs |   8.767 μs |   8.200 μs |   657.72 μs |  1.04 |    0.01 |  79.1016 | 78.1250 | 39.0625 |  312.4 KB |        1.06 |
|    XmlSerializer | NET_472 | .NET Framework 4.7.2 |   Serialization |   894.15 μs |   9.717 μs |   9.090 μs |   893.19 μs |  1.41 |    0.02 | 103.5156 | 80.0781 | 41.0156 | 478.31 KB |        1.63 |
| DataContractJson | NET_472 | .NET Framework 4.7.2 |   Serialization |   855.62 μs |   9.279 μs |   8.680 μs |   853.21 μs |  1.35 |    0.02 |  34.1797 | 34.1797 | 34.1797 | 227.64 KB |        0.78 |
|         Utf8Json | NET_472 | .NET Framework 4.7.2 |   Serialization |   378.24 μs |   3.515 μs |   3.288 μs |   379.11 μs |  0.60 |    0.01 |  41.5039 | 41.5039 | 41.5039 | 197.34 KB |        0.67 |
|      MessagePack | NET_472 | .NET Framework 4.7.2 |   Serialization |   112.81 μs |   0.756 μs |   0.631 μs |   113.00 μs |  0.18 |    0.00 |  25.6348 |  4.2725 |       - | 106.21 KB |        0.36 |
|       BinaryPack | NET_472 | .NET Framework 4.7.2 |   Serialization |   128.17 μs |   0.806 μs |   0.715 μs |   128.09 μs |  0.20 |    0.00 |  20.0195 |  3.1738 |       - |  83.33 KB |        0.28 |
|                  |         |                      |                 |             |            |            |             |       |         |          |         |         |           |             |
|   NewtonsoftJson |   NET_7 |             .NET 7.0 |   Serialization |   459.78 μs |   5.719 μs |   5.350 μs |   460.20 μs |  1.00 |    0.00 |  41.5039 | 41.5039 | 41.5039 | 291.54 KB |        1.00 |
|  BinaryFormatter |   NET_7 |             .NET 7.0 |   Serialization |   400.47 μs |   3.118 μs |   2.764 μs |   401.03 μs |  0.87 |    0.01 |  62.0117 | 14.6484 |       - | 255.13 KB |        0.88 |
|      NetCoreJson |   NET_7 |             .NET 7.0 |   Serialization |   401.95 μs |   3.511 μs |   3.112 μs |   402.56 μs |  0.87 |    0.01 |  83.0078 | 82.5195 | 41.5039 | 320.12 KB |        1.10 |
|    XmlSerializer |   NET_7 |             .NET 7.0 |   Serialization |   658.70 μs |   9.517 μs |   8.903 μs |   658.38 μs |  1.43 |    0.03 |  90.8203 | 90.8203 | 90.8203 | 420.74 KB |        1.44 |
| DataContractJson |   NET_7 |             .NET 7.0 |   Serialization |   683.30 μs |   7.291 μs |   6.820 μs |   684.48 μs |  1.49 |    0.02 |  35.1563 | 35.1563 | 35.1563 | 230.19 KB |        0.79 |
|         Utf8Json |   NET_7 |             .NET 7.0 |   Serialization |   365.78 μs |   4.568 μs |   4.050 μs |   366.56 μs |  0.80 |    0.01 |  41.5039 | 41.5039 | 41.5039 | 207.42 KB |        0.71 |
|      MessagePack |   NET_7 |             .NET 7.0 |   Serialization |    61.36 μs |   1.157 μs |   1.082 μs |    61.19 μs |  0.13 |    0.00 |  25.9399 |       - |       - | 106.95 KB |        0.37 |
|       BinaryPack |   NET_7 |             .NET 7.0 |   Serialization |    48.23 μs |   0.418 μs |   0.391 μs |    48.29 μs |  0.10 |    0.00 |  10.3760 |       - |       - |  42.99 KB |        0.15 |
