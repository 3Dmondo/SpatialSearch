using BenchmarkDotNet.Attributes;
using SpatialSearch;
using SpatialSearch.Abstractions;
using SpatialSearch.Tests;
using System.Runtime.Intrinsics;

#if DEBUG
Type[] types = [typeof(LinearSearch), typeof(QuadTree), typeof(KDTree)];
foreach (Type type in types)
{
  var benchmarkType = typeof(Benchmark<>).MakeGenericType(type);
  var benchmark = (Benchmark)Activator.CreateInstance(benchmarkType)!;
  int[] sizes = [1_000, 10_000, 100_000];
  foreach (int N in sizes)
  {
    benchmark.N = N;
    benchmark.GlobalSetup();
    Console.WriteLine($"{nameof(Benchmark)}<{type.Name}>.{nameof(Benchmark.FindNearest)}() = " +
      $"{benchmark.FindNearest()}");
    Console.WriteLine($"{nameof(Benchmark)}<{type.Name}>.{nameof(Benchmark.FindInRadius)}() = " +
      $"{benchmark.FindInRadius()}");
    Console.WriteLine($"{nameof(Benchmark)}<{type.Name}>.{nameof(Benchmark.Build)}() = " +
      $"{benchmark.Build()}");
  }
}
#else
BenchmarkSwitcher
  .FromTypes([typeof(Benchmark<>)])
  .RunAllJoined();
#endif


public abstract class Benchmark
{
  public abstract int N { get; set; }
  public abstract void GlobalSetup();
  public abstract Vector128<double> FindNearest();
  public abstract int FindInRadius();
  public abstract ISpatialSearch<SimplePoint> Build();
}

[MemoryDiagnoser]
[GenericTypeArguments(typeof(LinearSearch))]
[GenericTypeArguments(typeof(QuadTree))]
[GenericTypeArguments(typeof(KDTree))]
public class Benchmark<T> : Benchmark where T : ISpatialSearch
{
  Random Random = new Random(42);
  [Params(1_000, 10_000, 100_000)]
  public override int N { get; set; }
  double Radius;

  private SimplePoint[]? Points;
  private SimplePoint testPoint;
  private ISpatialSearch<SimplePoint>? SpatialSearch;

  [GlobalSetup]
  public override void GlobalSetup()
  {
    Radius = 2.0 / Math.Sqrt(N);
    Points = Enumerable
      .Range(0, N)
      .Select(_ => (SimplePoint)Vector128
        .Create(Random.NextDouble(), Random.NextDouble()))
      .ToArray();
    testPoint = Vector128.Create(Random.NextDouble(), Random.NextDouble());
    SpatialSearch = T.Build(Points);
  }

  [Benchmark()]
  public override Vector128<double> FindNearest()
  {
    return SpatialSearch!.FindNearest(testPoint).Item1;
  }

  [Benchmark()]
  public override int FindInRadius()
  {
    return SpatialSearch!.FindInRadius(testPoint, Radius).Count();
  }

  [Benchmark()]
  public override ISpatialSearch<SimplePoint> Build()
  {
    return T.Build(Points!);
  }
}
