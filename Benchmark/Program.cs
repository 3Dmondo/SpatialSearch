using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using SpatialSearch;
using SpatialSearch.Abstractions;
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

  private SimplePoint[]? Points;
  private SimplePoint testPoint;
  private INearestPointFinder<SimplePoint>? QuadTreeRoot;
  private INearestPointFinder<SimplePoint>? KDTreeRoot;

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
  }

  [Benchmark(Baseline = true)]
  public Vector128<double> FindNearestLinear()
  {
    return Points!.OrderBy(p => VectorExtensions.DistanceSquared(p, testPoint)).First();
  }

  //[Benchmark()]
  public Vector128<double> BuildQuadTree()
  {
    var treeRoot = QuadTree.Build(Points!);
    return treeRoot.FindNearest(testPoint).Item1;
  }

  [Benchmark()]
  public Vector128<double> FindNearestQuadTree()
  {
    return QuadTreeRoot!.FindNearest(testPoint).Item1;
  }

  //[Benchmark()]
  public Vector128<double> BuildKDTree()
  {
    var treeRoot = KDTree.Build(Points!);
    return treeRoot.FindNearest(testPoint).Item1;
  }

  [Benchmark()]
  public Vector128<double> FindNearestKDTree()
  {
    return KDTreeRoot!.FindNearest(testPoint).Item1;
  }
}