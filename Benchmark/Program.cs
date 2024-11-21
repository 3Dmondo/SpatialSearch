using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

#if !DEBUG
using BenchmarkDotNet.Running;
#endif
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
[GenericTypeArguments(typeof(BasicSearch))]
[GenericTypeArguments(typeof(QuadTree))]
[GenericTypeArguments(typeof(KDTree))]
public class Benchmark<T> : Benchmark where T : ISpatialSearch
{
  const int Iterations = 10;
  [Params(1_000, 10_000, 100_000)]
  public override int N { get; set; }
  double Radius;

  private SimplePoint[]? Points;
  private SimplePoint[] testPoints;
  private ISpatialSearch<SimplePoint>? SpatialSearch;
  private PointsGenerator PointsGenerator;

  [GlobalSetup]
  public override void GlobalSetup()
  {
    PointsGenerator = new PointsGenerator(1.0, 42);
    Radius = 2.0 / Math.Sqrt(N);
    Points = PointsGenerator.GeneratePoints(N).ToArray();
    testPoints = PointsGenerator.GeneratePoints(Iterations).ToArray();
    SpatialSearch = T.Build(Points);
  }

  [Benchmark(OperationsPerInvoke = Iterations)]
  public override Vector128<double> FindNearest()
  {
    var result = Vector128<double>.Zero;
    for (int i = 0; i < Iterations; i++)
      result += SpatialSearch!.FindNearest(testPoints[i]).Item1;
    return result;
  }

  [Benchmark(OperationsPerInvoke = Iterations)]
  public override int FindInRadius()
  {
    var result = 0;
    for (int i = 0; i < Iterations; i++)
      result += SpatialSearch!.FindInRadius(testPoints[i], Radius).Count();
    return result;
  }

  [Benchmark()]
  public override ISpatialSearch<SimplePoint> Build()
  {
    return T.Build(Points!);
  }
}
