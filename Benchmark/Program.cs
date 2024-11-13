﻿
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using QuadTree;
using QuadTree.Extensions;
using System.Runtime.Intrinsics;

BenchmarkRunner.Run<QuadTreeBenchmark>();

public class QuadTreeBenchmark
{
  Random Random = new Random(42);
  [Params(1000, 10000, 100000)]
  public int N;

  private SimplePoint[]? Points;
  private Vector128<double> testPoint;
  private QuadTreeCell<SimplePoint>? Tree;

  [GlobalSetup]
  public void GlobalSetup()
  {
    Points = Enumerable
      .Range(0, N)
      .Select(_ => (SimplePoint)Vector128
        .Create(Random.NextDouble(), Random.NextDouble()))
      .ToArray();
    testPoint = Vector128.Create(Random.NextDouble(), Random.NextDouble());
    Tree = QuadTree.QuadTree.BuildQadTree(Points);
  }

  [Benchmark(Baseline = true)]
  public Vector128<double> FindNearestBase()
  {
    return Points!.OrderBy(p => VectorExtensions.DistanceSquared(p, testPoint)).First();
  }

  [Benchmark()]
  public Vector128<double> FindNearestQuadTreeWithInit()
  {
    var tree = QuadTree.QuadTree.BuildQadTree(Points!);
    return tree.FindNearest(testPoint).Item1;
  }

  [Benchmark()]
  public Vector128<double> FindNearestQuadTreeInitialized()
  {
    return Tree!.FindNearest(testPoint).Item1;
  }

}

public struct SimplePoint : IPoint
{
  private Vector128<double> Point { get; init; }

  public double X => Point[0];

  public double Y => Point[1];

  public SimplePoint(Vector128<double> point) => Point = point;
  public static implicit operator Vector128<double>(SimplePoint point) => point.Point;
  public static implicit operator SimplePoint(Vector128<double> point) => new SimplePoint(point);
}