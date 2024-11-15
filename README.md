# QuadTree
BenchmarkDotNet v0.14.0, Windows 11 (10.0.22621.4317/22H2/2022Update/SunValley2)
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK 8.0.403
  [Host]     : .NET 8.0.10 (8.0.1024.46610), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.10 (8.0.1024.46610), X64 RyuJIT AVX2


| Method                         | N       | Mean             | Error            | StdDev           | Ratio   | RatioSD |
|------------------------------- |-------- |-----------------:|-----------------:|-----------------:|--------:|--------:|
| FindNearestBase                | 10000   |      31,845.5 ns |        137.09 ns |        114.48 ns |   1.000 |    0.00 |
| FindNearestQuadTreeWithInit    | 10000   |   1,076,499.9 ns |     18,976.78 ns |     25,975.64 ns |  33.804 |    0.81 |
| FindNearestQuadTreeInitialized | 10000   |         182.9 ns |          0.83 ns |          0.65 ns |   0.006 |    0.00 |
|                                |         |                  |                  |                  |         |
|
| FindNearestBase                | 100000  |     317,737.8 ns |      1,435.25 ns |      1,198.50 ns |   1.000 |    0.01 |
| FindNearestQuadTreeWithInit    | 100000  |  33,322,158.2 ns |    630,896.65 ns |    675,052.34 ns | 104.875 |    2.10 |
| FindNearestQuadTreeInitialized | 100000  |         215.9 ns |          2.02 ns |          1.79 ns |   0.001 |    0.00 |
|                                |         |                  |                  |                  |         |
|
| FindNearestBase                | 1000000 |   3,174,353.4 ns |      7,262.65 ns |      6,793.49 ns |   1.000 |    0.00 |
| FindNearestQuadTreeWithInit    | 1000000 | 798,747,106.7 ns | 15,368,853.15 ns | 14,376,034.78 ns | 251.626 |    4.42 |
| FindNearestQuadTreeInitialized | 1000000 |         342.9 ns |          1.96 ns |          1.73 ns |   0.000 |    0.00 |