using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using SpatialSearch;
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
  double Range = 0.01;

  private SimplePoint[]? Points;
  private SimplePoint testPoint;
  private SpatialSearch.Abstractions.ISpatialSearch<SimplePoint>? QuadTreeRoot;
  private SpatialSearch.Abstractions.ISpatialSearch<SimplePoint>? KDTreeRoot;
  private SpatialSearch.Abstractions.ISpatialSearch<SimplePoint>? KDTreeQuickSelectRoot;

  [GlobalSetup]
  public void GlobalSetup()
  {
    Points = Enumerable
      .Range(0, N)
      .Select(_ => (SimplePoint)Vector128
        .Create(Random.NextDouble(), Random.NextDouble()))
      .ToArray();
    testPoint = Vector128.Create(Random.NextDouble(), Random.NextDouble());
    QuadTreeRoot = QuadTree.Build(Points);
    KDTreeRoot = KDTree.Build(Points);
    KDTreeQuickSelectRoot = KDTreeQuickSelect.Build(Points);
  }

  //[Benchmark()]
  public Vector128<double> FindNearestLinear()
  {
    return Points!.OrderBy(p => VectorExtensions.DistanceSquared(p, testPoint)).First();
  }

  //[Benchmark()]
  public Vector128<double> FindNearestQuadTree()
  {
    return QuadTreeRoot!.FindNearest(testPoint).Item1;
  }

  [Benchmark()]
  public Vector128<double> FindNearestKDTree()
  {
    return KDTreeRoot!.FindNearest(testPoint).Item1;
  }

  [Benchmark()]
  public Vector128<double> FindNearestKDTreeQuickSelect()
  {
    return KDTreeQuickSelectRoot!.FindNearest(testPoint).Item1;
  }

  //[Benchmark()]
  public int FindInRadiusLinear()
  {
    return Points!
      .Select(p => (p, VectorExtensions.Distance(p, testPoint)))
      .Where(p => p.Item2 <= Range)
      .Count();
  }

  [Benchmark()]
  public int FindInRadiusQuadTree()
  {
    return QuadTreeRoot!.FindInRadius(testPoint, Range).Count();
  }

  [Benchmark()]
  public int FindInRadiusKDTree()
  {
    return KDTreeRoot!.FindInRadius(testPoint, Range).Count();
  }

  [Benchmark()]
  public int FindInRadiusKDTreeQuickSelect()
  {
    return KDTreeQuickSelectRoot!.FindInRadius(testPoint, Range).Count();
  }

  [Benchmark()]
  public SpatialSearch.Abstractions.ISpatialSearch<SimplePoint> BuildQuadTree()
  {
    return QuadTree.Build(Points!);
  }

  [Benchmark()]
  public SpatialSearch.Abstractions.ISpatialSearch<SimplePoint> BuildKDTree()
  {
    return KDTree.Build(Points!);
  }

  [Benchmark()]
  public SpatialSearch.Abstractions.ISpatialSearch<SimplePoint> BuildKDTreeQuickSelect()
  {
    return KDTreeQuickSelect.Build(Points!);
  }

}