using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using QuadTree;
using QuadTree.Extensions;
using QuadTree.Tests;
using System.Runtime.Intrinsics;

BenchmarkRunner.Run<QuadTreeBenchmark>();

public class QuadTreeBenchmark
{
  Random Random = new Random(42);
  [Params(1000, 10000, 100000)]
  public int N;

  private SimplePoint[]? Points;
  private Vector128<double> testPoint;
  private QuadTreeCell<SimplePoint>? TreeRoot;

  [GlobalSetup]
  public void GlobalSetup()
  {
    Points = Enumerable
      .Range(0, N)
      .Select(_ => (SimplePoint)Vector128
        .Create(Random.NextDouble(), Random.NextDouble()))
      .ToArray();
    testPoint = Vector128.Create(Random.NextDouble(), Random.NextDouble());
    TreeRoot = QuadTreeBuilder.Instance.Build(Points);
  }

  [Benchmark(Baseline = true)]
  public Vector128<double> FindNearestBase()
  {
    return Points!.OrderBy(p => VectorExtensions.DistanceSquared(p, testPoint)).First();
  }

  [Benchmark()]
  public Vector128<double> FindNearestQuadTreeWithInit()
  {
    var treeRoot = QuadTreeBuilder.Instance.Build(Points!);
    return treeRoot.FindNearest(testPoint).Item1;
  }

  [Benchmark()]
  public Vector128<double> FindNearestQuadTreeInitialized()
  {
    return TreeRoot!.FindNearest(testPoint).Item1;
  }

}