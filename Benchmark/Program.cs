using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using SpatialSearch.Extensions;
using SpatialSearch.Tests;
using System.Runtime.Intrinsics;

BenchmarkRunner.Run<FindNearestBenchmark>();


[MemoryDiagnoser]
public class FindNearestBenchmark
{
  Random Random = new Random(42);
  [Params(1_000, 10_000, 100_000)]
  public int N;
  const double RadiusFactor = 0.001;
  double Radius;

  private SimplePoint[]? Points;
  private SimplePoint testPoint;
  private SpatialSearch.Abstractions.ISpatialSearch<SimplePoint>? QuadTree;
  private SpatialSearch.Abstractions.ISpatialSearch<SimplePoint>? KDTree;

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
    QuadTree = SpatialSearch.QuadTree.Build(Points);
    KDTree = SpatialSearch.KDTree.Build(Points);
  }

  [Benchmark()]
  public Vector128<double> FindNearestLinear()
  {
    return Points!
      .Select(p => (p, VectorExtensions.Distance(p, testPoint)))
      .MinBy(p => p.Item2).p;
  }

  [Benchmark()]
  public Vector128<double> FindNearestQuadTree()
  {
    return QuadTree!.FindNearest(testPoint).Item1;
  }

  [Benchmark()]
  public Vector128<double> FindNearestKDTree()
  {
    return KDTree!.FindNearest(testPoint).Item1;
  }

  [Benchmark()]
  public int FindInRadiusLinear()
  {
    return Points!
      .Select(p => (p, VectorExtensions.Distance(p, testPoint)))
      .Where(p => p.Item2 <= Radius)
      .Count();
  }

  [Benchmark()]
  public int FindInRadiusQuadTree()
  {
    return QuadTree!.FindInRadius(testPoint, Radius).Count();
  }

  [Benchmark()]
  public int FindInRadiusKDTree()
  {
    return KDTree!.FindInRadius(testPoint, Radius).Count();
  }

  [Benchmark()]
  public SpatialSearch.Abstractions.ISpatialSearch<SimplePoint> BuildQuadTree()
  {
    return SpatialSearch.QuadTree.Build(Points!);
  }

  [Benchmark()]
  public SpatialSearch.Abstractions.ISpatialSearch<SimplePoint> BuildKDTree()
  {
    return SpatialSearch.KDTree.Build(Points!);
  }

}