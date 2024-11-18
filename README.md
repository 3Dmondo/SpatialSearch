BenchmarkDotNet v0.14.0, Windows 11 (10.0.22621.4317/22H2/2022Update/SunValley2)
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK 8.0.403
  [Host]     : .NET 8.0.10 (8.0.1024.46610), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.10 (8.0.1024.46610), X64 RyuJIT AVX2


| Method               | N      | Mean             | Error           | StdDev          | Gen0       | Gen1      | Gen2     | Allocated   |
|--------------------- |------- |-----------------:|----------------:|----------------:|-----------:|----------:|---------:|------------:|
| FindNearestLinear    | 1000   |       3,040.8 ns |        17.04 ns |        15.94 ns |     0.0114 |         - |        - |       200 B |
| FindNearestQuadTree  | 1000   |         161.5 ns |         1.28 ns |         1.20 ns |     0.0019 |         - |        - |        32 B |
| FindNearestKDTree    | 1000   |         197.1 ns |         1.70 ns |         1.50 ns |     0.0019 |         - |        - |        32 B |
| FindInRadiusLinear   | 1000   |       8,867.2 ns |        32.78 ns |        25.59 ns |     0.0153 |         - |        - |       296 B |
| FindInRadiusQuadTree | 1000   |         182.0 ns |         3.58 ns |         3.51 ns |     0.0291 |         - |        - |       488 B |
| FindInRadiusKDTree   | 1000   |         165.0 ns |         2.44 ns |         2.16 ns |     0.0229 |         - |        - |       384 B |
| BuildQuadTree        | 1000   |      67,710.7 ns |     1,284.58 ns |     1,374.48 ns |    15.2588 |    5.0049 |        - |    256248 B |
| BuildKDTree          | 1000   |     600,793.9 ns |    10,893.83 ns |    10,190.09 ns |   137.6953 |   53.7109 |        - |   2316824 B |
| FindNearestLinear    | 10000  |      32,240.8 ns |       304.91 ns |       285.21 ns |          - |         - |        - |       200 B |
| FindNearestQuadTree  | 10000  |         273.3 ns |         1.55 ns |         1.45 ns |     0.0019 |         - |        - |        32 B |
| FindNearestKDTree    | 10000  |         282.1 ns |         3.83 ns |         3.58 ns |     0.0019 |         - |        - |        32 B |
| FindInRadiusLinear   | 10000  |      93,765.1 ns |     1,577.88 ns |     1,475.95 ns |          - |         - |        - |       296 B |
| FindInRadiusQuadTree | 10000  |         381.5 ns |         7.33 ns |        14.65 ns |     0.0291 |         - |        - |       488 B |
| FindInRadiusKDTree   | 10000  |         310.7 ns |         2.76 ns |         2.45 ns |     0.0319 |         - |        - |       536 B |
| BuildQuadTree        | 10000  |   1,107,950.5 ns |    12,514.77 ns |    11,094.02 ns |   156.2500 |   97.6563 |        - |   2626537 B |
| BuildKDTree          | 10000  |  13,354,761.6 ns |   264,898.53 ns |   371,350.77 ns |  1734.3750 |  906.2500 | 140.6250 |  27385391 B |
| FindNearestLinear    | 100000 |     318,558.0 ns |     2,473.48 ns |     2,192.67 ns |          - |         - |        - |       200 B |
| FindNearestQuadTree  | 100000 |         294.6 ns |         1.59 ns |         1.48 ns |     0.0019 |         - |        - |        32 B |
| FindNearestKDTree    | 100000 |         318.1 ns |         2.10 ns |         1.87 ns |     0.0019 |         - |        - |        32 B |
| FindInRadiusLinear   | 100000 |     885,053.2 ns |     7,065.27 ns |     5,899.82 ns |          - |         - |        - |       296 B |
| FindInRadiusQuadTree | 100000 |         974.9 ns |        10.15 ns |         9.00 ns |     0.0458 |         - |        - |       768 B |
| FindInRadiusKDTree   | 100000 |         915.9 ns |         8.89 ns |         8.32 ns |     0.0315 |         - |        - |       536 B |
| BuildQuadTree        | 100000 |  31,139,309.6 ns |   608,810.13 ns |   569,481.38 ns |  1687.5000 | 1468.7500 | 125.0000 |  26156740 B |
| BuildKDTree          | 100000 | 196,313,842.2 ns | 3,471,589.81 ns | 3,247,327.26 ns | 17333.3333 | 8000.0000 | 333.3333 | 315104077 B |