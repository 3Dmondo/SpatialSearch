using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using SpatialSearch;
using SpatialSearch.Abstractions;
using SpatialSearch.Tests;
using System.Runtime.Intrinsics;

BenchmarkSwitcher.FromTypes([typeof(FindNearestBenchmark<>)]).RunAllJoined();

[MemoryDiagnoser]
[GenericTypeArguments(typeof(LinearSearch))]
[GenericTypeArguments(typeof(QuadTree))]
[GenericTypeArguments(typeof(KDTree))]
public class FindNearestBenchmark<T> where T : ISpatialSearch
{
  Random Random = new Random(42);
  [Params(1_000, 10_000, 100_000)]
  public int N;
  const double RadiusFactor = 0.001;
  double Radius;

  private SimplePoint[]? Points;
  private SimplePoint testPoint;
  private ISpatialSearch<SimplePoint>? SpatialSearch;

  [GlobalSetup]
  public void GlobalSetup()
  {
    Radius = RadiusFactor / N;
    Points = Enumerable
      .Range(0, N)
      .Select(_ => (SimplePoint)Vector128
        .Create(Random.NextDouble(), Random.NextDouble()))
      .ToArray();
    testPoint = Vector128.Create(Random.NextDouble(), Random.NextDouble());
    SpatialSearch = T.Build(Points);
  }

  [Benchmark()]
  public Vector128<double> FindNearest()
  {
    return SpatialSearch!.FindNearest(testPoint).Item1;
  }


  [Benchmark()]
  public int FindInRadius()
  {
    return SpatialSearch!.FindInRadius(testPoint, Radius).Count();
  }


  [Benchmark()]
  public ISpatialSearch<SimplePoint> Build()
  {
    return T.Build(Points!);
  }
}
