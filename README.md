BenchmarkDotNet v0.14.0, Windows 11 (10.0.22621.4317/22H2/2022Update/SunValley2)
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK 8.0.404
  [Host]     : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2


| Method               | N      | Mean            | Error           | StdDev          | Gen0      | Gen1      | Gen2     | Allocated   |
|--------------------- |------- |----------------:|----------------:|----------------:|----------:|----------:|---------:|------------:|
| FindNearestLinear    | 1000   |      8,718.3 ns |        66.58 ns |        59.02 ns |         - |         - |        - |       144 B |
| FindNearestQuadTree  | 1000   |        163.2 ns |         1.43 ns |         1.33 ns |    0.0019 |         - |        - |        32 B |
| FindNearestKDTree    | 1000   |        210.6 ns |         0.91 ns |         0.85 ns |    0.0019 |         - |        - |        32 B |
| FindInRadiusLinear   | 1000   |      8,962.1 ns |        21.54 ns |        19.09 ns |    0.0153 |         - |        - |       296 B |
| FindInRadiusQuadTree | 1000   |        173.4 ns |         2.00 ns |         1.87 ns |    0.0291 |         - |        - |       488 B |
| FindInRadiusKDTree   | 1000   |        184.5 ns |         2.50 ns |         2.33 ns |    0.0229 |         - |        - |       384 B |
| BuildQuadTree        | 1000   |     74,797.4 ns |     1,462.01 ns |     1,367.57 ns |   15.2588 |    7.4463 |        - |    256216 B |
| BuildKDTree          | 1000   |    195,659.6 ns |     3,648.44 ns |     3,412.76 ns |   47.1191 |   24.6582 |        - |    791048 B |
| FindNearestLinear    | 10000  |     87,054.9 ns |       759.19 ns |       710.14 ns |         - |         - |        - |       144 B |
| FindNearestQuadTree  | 10000  |        268.3 ns |         2.84 ns |         2.65 ns |    0.0019 |         - |        - |        32 B |
| FindNearestKDTree    | 10000  |        296.5 ns |         4.56 ns |         3.80 ns |    0.0019 |         - |        - |        32 B |
| FindInRadiusLinear   | 10000  |     88,377.3 ns |       270.13 ns |       210.90 ns |         - |         - |        - |       296 B |
| FindInRadiusQuadTree | 10000  |        245.8 ns |         4.71 ns |         4.63 ns |    0.0291 |         - |        - |       488 B |
| FindInRadiusKDTree   | 10000  |        238.1 ns |         1.93 ns |         1.61 ns |    0.0319 |         - |        - |       536 B |
| BuildQuadTree        | 10000  |  1,106,959.3 ns |    13,776.85 ns |    12,212.82 ns |  156.2500 |   97.6563 |        - |   2626505 B |
| BuildKDTree          | 10000  |  3,469,082.5 ns |    37,983.80 ns |    35,530.07 ns |  558.5938 |  410.1563 |  31.2500 |   8995540 B |
| FindNearestLinear    | 100000 |    862,964.5 ns |     5,445.25 ns |     4,827.07 ns |         - |         - |        - |       144 B |
| FindNearestQuadTree  | 100000 |        298.5 ns |         1.18 ns |         0.99 ns |    0.0019 |         - |        - |        32 B |
| FindNearestKDTree    | 100000 |        354.3 ns |         1.70 ns |         1.42 ns |    0.0019 |         - |        - |        32 B |
| FindInRadiusLinear   | 100000 |    894,664.0 ns |     8,073.60 ns |     6,741.82 ns |         - |         - |        - |       296 B |
| FindInRadiusQuadTree | 100000 |        264.4 ns |         5.21 ns |         4.87 ns |    0.0291 |         - |        - |       488 B |
| FindInRadiusKDTree   | 100000 |        278.0 ns |         5.38 ns |         5.03 ns |    0.0319 |         - |        - |       536 B |
| BuildQuadTree        | 100000 | 33,447,189.7 ns |   666,171.83 ns |   866,211.62 ns | 1718.7500 | 1500.0000 | 156.2500 |  26156480 B |
| BuildKDTree          | 100000 | 83,883,915.6 ns | 1,654,047.19 ns | 1,547,196.77 ns | 6166.6667 | 3333.3333 | 333.3333 | 100605711 B |