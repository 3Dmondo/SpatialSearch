```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22621.4317/22H2/2022Update/SunValley2)
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK 8.0.404
  [Host]     : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2


```
| Type                   | Method       | N      | Mean              | Error             | StdDev            | Median            | Gen0      | Gen1      | Gen2     | Allocated   |
|----------------------- |------------- |------- |------------------:|------------------:|------------------:|------------------:|----------:|----------:|---------:|------------:|
| **Benchmark&lt;BasicSearch&gt;** | **FindNearest**  | **1000**   |      **9,306.096 ns** |        **66.8458 ns** |        **59.2571 ns** |      **9,279.300 ns** |         **-** |         **-** |        **-** |       **200 B** |
| Benchmark&lt;KDTree&gt;      | FindNearest  | 1000   |        213.724 ns |         1.4956 ns |         1.3990 ns |        213.500 ns |    0.0019 |         - |        - |        32 B |
| Benchmark&lt;QuadTree&gt;    | FindNearest  | 1000   |        153.084 ns |         0.9723 ns |         0.8619 ns |        152.925 ns |    0.0019 |         - |        - |        32 B |
| Benchmark&lt;BasicSearch&gt; | FindInRadius | 1000   |      9,602.224 ns |        65.5339 ns |        61.3005 ns |      9,595.107 ns |    0.0122 |         - |        - |       360 B |
| Benchmark&lt;KDTree&gt;      | FindInRadius | 1000   |        619.554 ns |         9.1274 ns |         7.6218 ns |        619.097 ns |    0.0626 |         - |        - |      1049 B |
| Benchmark&lt;QuadTree&gt;    | FindInRadius | 1000   |        307.225 ns |         0.6096 ns |         0.4760 ns |        307.223 ns |    0.0206 |         - |        - |       350 B |
| Benchmark&lt;BasicSearch&gt; | Build        | 1000   |          9.649 ns |         0.4904 ns |         1.4461 ns |          9.781 ns |    0.0014 |         - |        - |        24 B |
| Benchmark&lt;KDTree&gt;      | Build        | 1000   |    198,360.466 ns |     3,931.5908 ns |     9,863.5934 ns |    193,893.384 ns |   51.0254 |   28.5645 |        - |    855000 B |
| Benchmark&lt;QuadTree&gt;    | Build        | 1000   |     87,101.435 ns |     1,513.6068 ns |     2,444.1930 ns |     86,208.539 ns |   16.8457 |    8.9111 |        - |    282672 B |
| **Benchmark&lt;BasicSearch&gt;** | **FindNearest**  | **10000**  |     **90,835.944 ns** |       **412.7610 ns** |       **344.6742 ns** |     **90,685.996 ns** |         **-** |         **-** |        **-** |       **200 B** |
| Benchmark&lt;KDTree&gt;      | FindNearest  | 10000  |        281.600 ns |         2.0822 ns |         1.9477 ns |        281.002 ns |    0.0019 |         - |        - |        32 B |
| Benchmark&lt;QuadTree&gt;    | FindNearest  | 10000  |        227.027 ns |         1.5928 ns |         1.4120 ns |        226.439 ns |    0.0019 |         - |        - |        32 B |
| Benchmark&lt;BasicSearch&gt; | FindInRadius | 10000  |     93,788.207 ns |       466.1216 ns |       413.2047 ns |     93,823.130 ns |         - |         - |        - |       360 B |
| Benchmark&lt;KDTree&gt;      | FindInRadius | 10000  |        812.907 ns |        11.9725 ns |        10.6133 ns |        815.163 ns |    0.0732 |         - |        - |      1246 B |
| Benchmark&lt;QuadTree&gt;    | FindInRadius | 10000  |        412.713 ns |         3.7611 ns |         3.3341 ns |        412.535 ns |    0.0229 |         - |        - |       383 B |
| Benchmark&lt;BasicSearch&gt; | Build        | 10000  |          7.070 ns |         0.2854 ns |         0.8414 ns |          7.170 ns |    0.0014 |         - |        - |        24 B |
| Benchmark&lt;KDTree&gt;      | Build        | 10000  |  3,487,069.448 ns |    40,543.9366 ns |    35,941.1437 ns |  3,489,325.586 ns |  574.2188 |  347.6563 |  46.8750 |   9635497 B |
| Benchmark&lt;QuadTree&gt;    | Build        | 10000  |  1,284,573.796 ns |    15,386.2326 ns |    12,012.5583 ns |  1,280,294.922 ns |  171.8750 |  113.2813 |        - |   2903809 B |
| **Benchmark&lt;BasicSearch&gt;** | **FindNearest**  | **100000** |    **911,420.144 ns** |     **6,546.3932 ns** |     **5,466.5348 ns** |    **911,858.750 ns** |         **-** |         **-** |        **-** |       **201 B** |
| Benchmark&lt;KDTree&gt;      | FindNearest  | 100000 |        337.897 ns |         1.9416 ns |         1.6214 ns |        337.724 ns |    0.0019 |         - |        - |        32 B |
| Benchmark&lt;QuadTree&gt;    | FindNearest  | 100000 |        278.115 ns |         2.0964 ns |         1.8584 ns |        277.809 ns |    0.0019 |         - |        - |        32 B |
| Benchmark&lt;BasicSearch&gt; | FindInRadius | 100000 |    938,720.022 ns |     8,313.9328 ns |     7,370.0849 ns |    936,978.047 ns |         - |         - |        - |       361 B |
| Benchmark&lt;KDTree&gt;      | FindInRadius | 100000 |        893.653 ns |        16.7471 ns |        15.6652 ns |        894.923 ns |    0.0687 |         - |        - |      1167 B |
| Benchmark&lt;QuadTree&gt;    | FindInRadius | 100000 |        390.732 ns |         1.8546 ns |         1.6440 ns |        390.775 ns |    0.0214 |         - |        - |       359 B |
| Benchmark&lt;BasicSearch&gt; | Build        | 100000 |          5.086 ns |         0.1468 ns |         0.4020 ns |          5.129 ns |    0.0014 |         - |        - |        24 B |
| Benchmark&lt;KDTree&gt;      | Build        | 100000 | 93,764,522.868 ns | 1,872,321.2251 ns | 5,093,788.3513 ns | 93,041,050.000 ns | 6666.6667 | 3500.0000 | 500.0000 | 107005717 B |
| Benchmark&lt;QuadTree&gt;    | Build        | 100000 | 44,288,571.795 ns |   809,560.6768 ns |   676,019.8305 ns | 44,295,325.000 ns | 2083.3333 | 2000.0000 | 416.6667 |  28909887 B |
