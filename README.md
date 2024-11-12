# QuadTree
// * Summary *

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22621.4317/22H2/2022Update/SunValley2)
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK 8.0.403
  [Host]     : .NET 8.0.10 (8.0.1024.46610), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.10 (8.0.1024.46610), X64 RyuJIT AVX2


| Method                         | N      | Mean             | Error          | StdDev         | Ratio   | RatioSD |
|------------------------------- |------- |-----------------:|---------------:|---------------:|--------:|--------:|
| FindNearestBase                | 1000   |      3,047.73 ns |       6.662 ns |       5.201 ns |    1.00 |    0.00 |
| FindNearestQuadTreeWithInit    | 1000   |     66,257.53 ns |   1,098.881 ns |   1,027.894 ns |   21.74 |    0.33 |
| FindNearestQuadTreeInitialized | 1000   |         87.17 ns |       0.203 ns |       0.180 ns |    0.03 |    0.00 |
|                                |        |                  |                |                |         |         |
| FindNearestBase                | 10000  |     29,885.30 ns |     198.324 ns |     185.512 ns |   1.000 |    0.01 |
| FindNearestQuadTreeWithInit    | 10000  |  1,047,466.58 ns |   6,878.796 ns |   5,744.106 ns |  35.051 |    0.28 |
| FindNearestQuadTreeInitialized | 10000  |        137.52 ns |       0.417 ns |       0.370 ns |   0.005 |    0.00 |
|                                |        |                  |                |                |         |         |
| FindNearestBase                | 100000 |    319,462.15 ns |   1,127.905 ns |     999.858 ns |   1.000 |    0.00 |
| FindNearestQuadTreeWithInit    | 100000 | 32,065,242.28 ns | 562,314.233 ns | 577,455.318 ns | 100.373 |    1.78 |
| FindNearestQuadTreeInitialized | 100000 |        132.63 ns |       0.477 ns |       0.423 ns |   0.000 |    0.00 |