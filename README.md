```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22621.4317/22H2/2022Update/SunValley2)
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK 8.0.404
  [Host]     : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2


```
| Type                   | Method       | N      | Mean              | Error             | StdDev            | Median            | Gen0      | Gen1      | Gen2     | Allocated   |
|----------------------- |------------- |------- |------------------:|------------------:|------------------:|------------------:|----------:|----------:|---------:|------------:|
| **Benchmark&lt;BasicSearch&gt;** | **FindNearest**  | **1000**   |      **9,300.658 ns** |        **98.9852 ns** |        **87.7478 ns** |      **9,314.682 ns** |         **-** |         **-** |        **-** |       **200 B** |
| Benchmark&lt;KDTree&gt;      | FindNearest  | 1000   |        245.299 ns |         2.3248 ns |         2.0609 ns |        244.647 ns |    0.0019 |         - |        - |        32 B |
| Benchmark&lt;QuadTree&gt;    | FindNearest  | 1000   |        165.101 ns |         0.9456 ns |         0.8383 ns |        165.226 ns |    0.0019 |         - |        - |        32 B |
| Benchmark&lt;BasicSearch&gt; | FindInRadius | 1000   |      9,551.566 ns |        60.4763 ns |        56.5696 ns |      9,537.167 ns |    0.0122 |         - |        - |       360 B |
| Benchmark&lt;KDTree&gt;      | FindInRadius | 1000   |        593.157 ns |         7.4179 ns |         6.1943 ns |        591.056 ns |    0.0275 |         - |        - |       460 B |
| Benchmark&lt;QuadTree&gt;    | FindInRadius | 1000   |        439.653 ns |         4.8243 ns |         4.5126 ns |        439.390 ns |    0.0267 |         - |        - |       458 B |
| Benchmark&lt;BasicSearch&gt; | Build        | 1000   |          8.269 ns |         0.4025 ns |         1.1868 ns |          8.583 ns |    0.0014 |         - |        - |        24 B |
| Benchmark&lt;KDTree&gt;      | Build        | 1000   |    184,376.971 ns |     2,124.3262 ns |     1,883.1598 ns |    184,465.198 ns |   47.1191 |   24.6582 |        - |    791048 B |
| Benchmark&lt;QuadTree&gt;    | Build        | 1000   |     69,805.913 ns |       929.9913 ns |       824.4131 ns |     69,757.825 ns |   15.2588 |    7.4463 |        - |    256216 B |
| **Benchmark&lt;BasicSearch&gt;** | **FindNearest**  | **10000**  |     **90,476.378 ns** |       **699.1695 ns** |       **619.7956 ns** |     **90,191.470 ns** |         **-** |         **-** |        **-** |       **200 B** |
| Benchmark&lt;KDTree&gt;      | FindNearest  | 10000  |        365.477 ns |         2.6469 ns |         2.4759 ns |        365.140 ns |    0.0019 |         - |        - |        32 B |
| Benchmark&lt;QuadTree&gt;    | FindNearest  | 10000  |        263.140 ns |         1.8664 ns |         1.7459 ns |        263.147 ns |    0.0019 |         - |        - |        32 B |
| Benchmark&lt;BasicSearch&gt; | FindInRadius | 10000  |     93,903.031 ns |       686.7035 ns |       608.7448 ns |     93,869.990 ns |         - |         - |        - |       360 B |
| Benchmark&lt;KDTree&gt;      | FindInRadius | 10000  |        781.839 ns |        14.1640 ns |        13.2490 ns |        780.705 ns |    0.0313 |         - |        - |       536 B |
| Benchmark&lt;QuadTree&gt;    | FindInRadius | 10000  |        626.657 ns |         8.1832 ns |         7.6545 ns |        626.203 ns |    0.0374 |         - |        - |       628 B |
| Benchmark&lt;BasicSearch&gt; | Build        | 10000  |          5.840 ns |         0.2053 ns |         0.6053 ns |          5.868 ns |    0.0014 |         - |        - |        24 B |
| Benchmark&lt;KDTree&gt;      | Build        | 10000  |  3,201,936.556 ns |    23,221.6752 ns |    18,129.9565 ns |  3,202,483.398 ns |  546.8750 |  351.5625 |  46.8750 |   8995545 B |
| Benchmark&lt;QuadTree&gt;    | Build        | 10000  |  1,105,811.412 ns |    19,234.7515 ns |    17,051.1062 ns |  1,104,319.922 ns |  156.2500 |   97.6563 |        - |   2626505 B |
| **Benchmark&lt;BasicSearch&gt;** | **FindNearest**  | **100000** |    **914,200.000 ns** |     **7,951.2290 ns** |     **7,437.5845 ns** |    **914,833.594 ns** |         **-** |         **-** |        **-** |       **201 B** |
| Benchmark&lt;KDTree&gt;      | FindNearest  | 100000 |        445.732 ns |         3.3558 ns |         3.1391 ns |        444.399 ns |    0.0015 |         - |        - |        32 B |
| Benchmark&lt;QuadTree&gt;    | FindNearest  | 100000 |        307.419 ns |         1.9826 ns |         1.8546 ns |        307.683 ns |    0.0019 |         - |        - |        32 B |
| Benchmark&lt;BasicSearch&gt; | FindInRadius | 100000 |    941,666.760 ns |    13,024.6859 ns |    12,183.2993 ns |    942,718.750 ns |         - |         - |        - |       361 B |
| Benchmark&lt;KDTree&gt;      | FindInRadius | 100000 |        877.412 ns |        17.4640 ns |        20.7896 ns |        881.211 ns |    0.0305 |         - |        - |       521 B |
| Benchmark&lt;QuadTree&gt;    | FindInRadius | 100000 |        631.139 ns |        12.0129 ns |        11.7983 ns |        633.731 ns |    0.0336 |         - |        - |       572 B |
| Benchmark&lt;BasicSearch&gt; | Build        | 100000 |          4.739 ns |         0.1394 ns |         0.2441 ns |          4.698 ns |    0.0014 |         - |        - |        24 B |
| Benchmark&lt;KDTree&gt;      | Build        | 100000 | 90,763,467.391 ns | 1,733,663.9493 ns | 2,192,529.9893 ns | 91,242,583.333 ns | 6666.6667 | 3833.3333 | 833.3333 | 100605872 B |
| Benchmark&lt;QuadTree&gt;    | Build        | 100000 | 32,534,968.750 ns |   633,771.3138 ns |   704,435.1362 ns | 32,469,131.250 ns | 1875.0000 | 1625.0000 | 312.5000 |  26156978 B |
