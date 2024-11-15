using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using SpatialSearch;
using SpatialSearch.Abstractions;
using SpatialSearch.Extensions;
using SpatialSearch.Tests;
using System.Runtime.Intrinsics;

BenchmarkRunner.Run<QuadTreeBenchmark>();

public class QuadTreeBenchmark
{
  Random Random = new Random(42);
  [Params(10_000, 100_000, 1_000_000)]
  public int N;

  private SimplePoint[]? Points;
  private SimplePoint testPoint;
  private INearestPointFinder<SimplePoint>? TreeRoot;

  [GlobalSetup]
  public void GlobalSetup()
  {
    Points = Enumerable
      .Range(0, N)
      .Select(_ => (SimplePoint)Vector128
        .Create(Random.NextDouble(), Random.NextDouble()))
      .ToArray();
    testPoint = Vector128.Create(Random.NextDouble(), Random.NextDouble());
    TreeRoot = QuadTree.Build(Points);
  }

  [Benchmark(Baseline = true)]
  public Vector128<double> FindNearestBase()
  {
    return Points!.OrderBy(p => VectorExtensions.DistanceSquared(p, testPoint)).First();
  }

  [Benchmark()]
  public Vector128<double> FindNearestQuadTreeWithInit()
  {
    var treeRoot = QuadTree.Build(Points!);
    return treeRoot.FindNearest(testPoint).Item1;
  }

  [Benchmark()]
  public Vector128<double> FindNearestQuadTreeInitialized()
  {
    return TreeRoot!.FindNearest(testPoint).Item1;
  }

}