using NUnit.Framework;
using SpatialSearch.Abstractions;
using SpatialSearch.Extensions;
using System.Runtime.Intrinsics;

namespace SpatialSearch.Tests;

[TestFixture(typeof(QuadTree))]
public class INearestPointFinderTests<T>
  where T : INearestPointFinder
{

  [Test, Combinatorial]
  public void FindNearest(
    [Values(1_000, 10_000, 100_000)] int numberOfPoints,
    [Values(0.1, 1.0, 10.0, 100.0)] double size,
    [Values(0, 1, 42)] int randomSeed)
  {
    var random = new Random(randomSeed);
    var points = Enumerable
      .Range(0, numberOfPoints)
      .Select(_ => (SimplePoint)Vector128.Create(
        random.NextDouble() * size,
        random.NextDouble() * size))
      .ToList();
    var treeRoot = T.Build(points);
    int Tries = 10;
    while (Tries-- > 0)
    {
      SimplePoint testPoint = Vector128.Create(
        random.NextDouble() * size,
        random.NextDouble() * size);
      var expected = points
        .Select(p => (p, Math.Sqrt(VectorExtensions.DistanceSquared(p, testPoint))))
        .OrderBy(p => p.Item2).First();
      var nearest = treeRoot.FindNearest(testPoint);
      Assert.That(nearest, Is.EqualTo(expected), $"Failed at the {Tries} iteration");
    }
  }

  [Test]
  public void FindNearestMinDistance()
  {
    var random = new Random(42);
    var points = Enumerable
     .Range(0, 100)
     .Select(_ => (SimplePoint)Vector128.Create(
       random.NextDouble(),
       random.NextDouble()))
     .ToList();
    var testPoint = (SimplePoint)Vector128.Create(2.0, 0.0);
    var treeRoot = T.Build(points);
    var result = treeRoot.TryFindNearest(testPoint, 1.0, out var nearest);
    Assert.That(result, Is.False);
    Assert.That(nearest.Point, Is.Default);
    Assert.That(nearest.Distance, Is.EqualTo(double.MaxValue));
  }

  [Test]
  public void FindNearestFar()
  {
    var random = new Random(42);
    var points = Enumerable
     .Range(0, 100)
     .Select(_ => (SimplePoint)Vector128.Create(
       random.NextDouble(),
       random.NextDouble()))
     .ToList();
    var testPoint = (SimplePoint)Vector128.Create(20.0, 0.0);
    var expected = points
      .Select(p => (p, Math.Sqrt(VectorExtensions.DistanceSquared(p, testPoint))))
      .OrderBy(p => p.Item2).First();
    var treeRoot = T.Build(points);
    var nearest = treeRoot.FindNearest(testPoint);
    Assert.That(nearest, Is.EqualTo(expected));
  }
}
