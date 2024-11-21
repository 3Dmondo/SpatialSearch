BenchmarkDotNet v0.14.0, Windows 11 (10.0.22621.4317/22H2/2022Update/SunValley2)
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK 8.0.404
  [Host]     : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2


| Type                   | Method       | N      | Mean              | Error             | StdDev            | Gen0      | Gen1      | Gen2     | Allocated   |
|----------------------- |------------- |------- |------------------:|------------------:|------------------:|----------:|----------:|---------:|------------:|
| Benchmark<BasicSearch> | FindNearest  | 1000   |      9,411.426 ns |        83.7919 ns |        78.3790 ns |         - |         - |        - |       200 B |
| Benchmark<KDTree>      | FindNearest  | 1000   |        245.550 ns |         2.4826 ns |         2.3222 ns |    0.0019 |         - |        - |        32 B |
| Benchmark<QuadTree>    | FindNearest  | 1000   |        167.715 ns |         1.7242 ns |         1.6128 ns |    0.0019 |         - |        - |        32 B |
| Benchmark<BasicSearch> | FindInRadius | 1000   |      9,772.474 ns |        51.1000 ns |        47.7989 ns |    0.0122 |         - |        - |       360 B |
| Benchmark<KDTree>      | FindInRadius | 1000   |        603.761 ns |         4.9215 ns |         4.6036 ns |    0.0275 |         - |        - |       460 B |
| Benchmark<QuadTree>    | FindInRadius | 1000   |        454.334 ns |         6.3193 ns |         5.6019 ns |    0.0267 |         - |        - |       458 B |
| Benchmark<BasicSearch> | Build        | 1000   |         11.140 ns |         0.3145 ns |         0.9272 ns |    0.0014 |         - |        - |        24 B |
| Benchmark<KDTree>      | Build        | 1000   |    193,253.582 ns |     3,711.6725 ns |     4,125.5142 ns |   47.1191 |   24.6582 |        - |    791048 B |
| Benchmark<QuadTree>    | Build        | 1000   |     71,240.163 ns |       973.8119 ns |       863.2589 ns |   15.2588 |    7.4463 |        - |    256216 B |
| Benchmark<BasicSearch> | FindNearest  | 10000  |     90,920.239 ns |       728.0057 ns |       680.9770 ns |         - |         - |        - |       200 B |
| Benchmark<KDTree>      | FindNearest  | 10000  |        373.518 ns |         1.6609 ns |         1.4724 ns |    0.0015 |         - |        - |        32 B |
| Benchmark<QuadTree>    | FindNearest  | 10000  |        263.177 ns |         1.4607 ns |         1.3663 ns |    0.0019 |         - |        - |        32 B |
| Benchmark<BasicSearch> | FindInRadius | 10000  |     93,609.606 ns |       531.9204 ns |       471.5335 ns |         - |         - |        - |       360 B |
| Benchmark<KDTree>      | FindInRadius | 10000  |        792.374 ns |        10.6360 ns |         9.9489 ns |    0.0305 |         - |        - |       536 B |
| Benchmark<QuadTree>    | FindInRadius | 10000  |        642.088 ns |        12.8118 ns |        12.5829 ns |    0.0374 |         - |        - |       628 B |
| Benchmark<BasicSearch> | Build        | 10000  |          8.241 ns |         0.5497 ns |         1.6209 ns |    0.0014 |         - |        - |        24 B |
| Benchmark<KDTree>      | Build        | 10000  |  3,332,483.411 ns |    50,729.9067 ns |    47,452.7862 ns |  546.8750 |  351.5625 |  46.8750 |   8995545 B |
| Benchmark<QuadTree>    | Build        | 10000  |  1,124,196.458 ns |    18,877.4765 ns |    28,254.9203 ns |  156.2500 |   97.6563 |        - |   2626505 B |
| Benchmark<BasicSearch> | FindNearest  | 100000 |    911,579.520 ns |     6,684.3301 ns |     5,925.4845 ns |         - |         - |        - |       201 B |
| Benchmark<KDTree>      | FindNearest  | 100000 |        445.160 ns |         3.4202 ns |         3.0319 ns |    0.0015 |         - |        - |        32 B |
| Benchmark<QuadTree>    | FindNearest  | 100000 |        312.886 ns |         2.1526 ns |         2.0135 ns |    0.0019 |         - |        - |        32 B |
| Benchmark<BasicSearch> | FindInRadius | 100000 |    930,261.875 ns |     7,482.7254 ns |     6,999.3460 ns |         - |         - |        - |       361 B |
| Benchmark<KDTree>      | FindInRadius | 100000 |        892.349 ns |        17.8498 ns |        16.6967 ns |    0.0305 |         - |        - |       521 B |
| Benchmark<QuadTree>    | FindInRadius | 100000 |        609.245 ns |         9.5217 ns |         8.9066 ns |    0.0336 |         - |        - |       572 B |
| Benchmark<BasicSearch> | Build        | 100000 |          6.761 ns |         0.4587 ns |         1.3524 ns |    0.0014 |         - |        - |        24 B |
| Benchmark<KDTree>      | Build        | 100000 | 96,215,233.333 ns | 1,923,357.7695 ns | 3,266,007.7217 ns | 6666.6667 | 3833.3333 | 833.3333 | 100605887 B |
| Benchmark<QuadTree>    | Build        | 100000 | 32,841,114.474 ns |   644,358.7548 ns |   716,203.0488 ns | 1875.0000 | 1625.0000 | 312.5000 |  26156978 B |