using Benchmark;
#if !DEBUG
using BenchmarkDotNet.Running;
#endif
using SpatialSearch;
using SpatialSearch.Tests;

#if DEBUG
Console.WriteLine(new GrandCircleBenchmark().GrandCircleDistancePoint());
Console.WriteLine(new GrandCircleBenchmark().GrandCircleDistanceVector());
Console.WriteLine(new GrandCircleBenchmark().GrandCircleDistanceVectorNet9());

//LogSearchBenchmark();
#else
BenchmarkRunner.Run<GrandCircleBenchmark>();

//BenchmarkSwitcher
//  .FromTypes([typeof(SearchBenchmark<>)])
//  .RunAllJoined();
#endif

static void LogSearchBenchmark()
{
  Type[] types = [typeof(BasicSearch), typeof(QuadTree), typeof(KDTree)];
  foreach (Type type in types)
  {
    var benchmarkType = typeof(SearchBenchmark<>).MakeGenericType(type);
    var benchmark = (ISearchBenchmark)Activator.CreateInstance(benchmarkType)!;
    int[] sizes = [1_000, 10_000, 100_000];
    double[] radiusFactors = [2.0, 4.0, 8.0];
    foreach (int N in sizes)
      foreach (double radiusFactor in radiusFactors)
      {
        benchmark.N = N;
        benchmark.RadiusFactor = radiusFactor;
        benchmark.GlobalSetup();
        Console.WriteLine($"{nameof(Benchmark)}<{type.Name}>.{nameof(ISearchBenchmark.FindNearest)}() = " +
          $"{benchmark.FindNearest()}");
        Console.WriteLine($"{nameof(Benchmark)}<{type.Name}>.{nameof(ISearchBenchmark.FindInRadius)}() = " +
          $"{benchmark.FindInRadius()}");
        Console.WriteLine($"{nameof(Benchmark)}<{type.Name}>.{nameof(ISearchBenchmark.Build)}() = " +
          $"{benchmark.Build()}");
      }
  }
}

