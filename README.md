BenchmarkDotNet v0.14.0, Windows 11 (10.0.22621.4317/22H2/2022Update/SunValley2)
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK 8.0.403
  [Host]     : .NET 8.0.10 (8.0.1024.46610), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.10 (8.0.1024.46610), X64 RyuJIT AVX2


| Method               | N      | Mean             | Error           | StdDev          | Gen0       | Gen1      | Gen2     | Allocated   |
|--------------------- |------- |-----------------:|----------------:|----------------:|-----------:|----------:|---------:|------------:|
| FindNearestLinear    | 1000   |       3,095.8 ns |        20.44 ns |        19.12 ns |     0.0114 |         - |        - |       200 B |
| FindNearestQuadTree  | 1000   |         163.8 ns |         0.89 ns |         0.83 ns |     0.0019 |         - |        - |        32 B |
| FindNearestKDTree    | 1000   |         200.0 ns |         0.25 ns |         0.21 ns |     0.0019 |         - |        - |        32 B |
| FindRangeLinear      | 1000   |       9,056.2 ns |        63.01 ns |        58.94 ns |     0.0153 |         - |        - |       296 B |
| FindInRadiusQuadTree | 1000   |         191.9 ns |         2.51 ns |         2.22 ns |     0.0291 |         - |        - |       488 B |
| FindInRadiusKDTree   | 1000   |         181.9 ns |         1.76 ns |         1.47 ns |     0.0229 |         - |        - |       384 B |
| BuildQuadTree        | 1000   |      73,839.6 ns |       793.06 ns |       741.83 ns |    15.2588 |    5.0049 |        - |    256248 B |
| BuildKDTree          | 1000   |     651,592.1 ns |     6,714.26 ns |     5,952.02 ns |   137.6953 |   53.7109 |        - |   2316824 B |
| FindNearestLinear    | 10000  |      30,260.5 ns |       178.34 ns |       139.24 ns |          - |         - |        - |       200 B |
| FindNearestQuadTree  | 10000  |         272.6 ns |         2.26 ns |         2.12 ns |     0.0019 |         - |        - |        32 B |
| FindNearestKDTree    | 10000  |         282.9 ns |         2.95 ns |         2.76 ns |     0.0019 |         - |        - |        32 B |
| FindRangeLinear      | 10000  |      90,368.5 ns |       724.43 ns |       642.19 ns |          - |         - |        - |       296 B |
| FindInRadiusQuadTree | 10000  |         388.5 ns |         2.33 ns |         2.18 ns |     0.0291 |         - |        - |       488 B |
| FindInRadiusKDTree   | 10000  |         320.5 ns |         3.94 ns |         3.69 ns |     0.0319 |         - |        - |       536 B |
| BuildQuadTree        | 10000  |   1,195,692.5 ns |     8,442.06 ns |     7,483.67 ns |   156.2500 |   97.6563 |        - |   2626537 B |
| BuildKDTree          | 10000  |  14,036,140.4 ns |   150,809.17 ns |   125,932.49 ns |  1734.3750 |  906.2500 | 140.6250 |  27385391 B |
| FindNearestLinear    | 100000 |     320,751.0 ns |       577.78 ns |       540.46 ns |          - |         - |        - |       200 B |
| FindNearestQuadTree  | 100000 |         308.5 ns |         2.15 ns |         1.79 ns |     0.0019 |         - |        - |        32 B |
| FindNearestKDTree    | 100000 |         323.3 ns |         1.57 ns |         1.39 ns |     0.0019 |         - |        - |        32 B |
| FindRangeLinear      | 100000 |     907,168.3 ns |    10,985.45 ns |    10,275.79 ns |          - |         - |        - |       296 B |
| FindInRadiusQuadTree | 100000 |       1,008.9 ns |         2.78 ns |         2.60 ns |     0.0458 |         - |        - |       768 B |
| FindInRadiusKDTree   | 100000 |         955.4 ns |         7.40 ns |         6.92 ns |     0.0315 |         - |        - |       536 B |
| BuildQuadTree        | 100000 |  33,119,504.4 ns |   642,676.77 ns |   835,661.41 ns |  1687.5000 | 1437.5000 | 125.0000 |  26156927 B |
| BuildKDTree          | 100000 | 214,977,519.6 ns | 4,193,676.82 ns | 4,306,597.34 ns | 17333.3333 | 8000.0000 | 333.3333 | 315102195 B |