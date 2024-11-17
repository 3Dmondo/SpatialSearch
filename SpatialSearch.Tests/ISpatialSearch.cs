using NUnit.Framework;
using SpatialSearch.Abstractions;
using SpatialSearch.Extensions;
using System.Runtime.Intrinsics;

namespace SpatialSearch.Tests;

[TestFixture(typeof(QuadTree))]
[TestFixture(typeof(KDTree))]
public class ISpatialSearch<TSpatialSearch>
  where TSpatialSearch : ISpatialSearch
{
  private const int Iterations = 10;

  [Test, Combinatorial]
  public void FindNearest(
    [Values(1_000, 10_000, 100_000)] int numberOfPoints,
    [Values(0.1, 1.0, 10.0, 100.0)] double size,
    [Values(0, 1, 42)] int randomSeed)
  {
    var random = new Random(randomSeed);
    var points = GeneratePoints(numberOfPoints, size, random);
    var treeRoot = TSpatialSearch.Build(points);
    int iteration = 0;
    while (iteration++ < Iterations)
    {
      SimplePoint testPoint = Vector128.Create(
        random.NextDouble() * size,
        random.NextDouble() * size);
      var expected = FindNearest(points, testPoint);
      var nearest = treeRoot.FindNearest(testPoint);
      Assert.That(nearest, Is.EqualTo(expected), $"Failed at iteration {iteration}");
    }
  }

  [Test]
  public void FindNearestMinDistance()
  {
    var random = new Random(42);
    var points = GeneratePoints(100, 1.0, random);
    var testPoint = (SimplePoint)Vector128.Create(2.0, 0.0);
    var treeRoot = TSpatialSearch.Build(points);
    var result = treeRoot.TryFindNearest(testPoint, 1.0, out var nearest);
    Assert.That(result, Is.False);
    Assert.That(nearest, Is.EqualTo((default(SimplePoint), double.MaxValue)));
  }

  [Test]
  public void FindNearestFar()
  {
    var random = new Random(42);
    var points = GeneratePoints(100, 1.0, random);
    var testPoint = (SimplePoint)Vector128.Create(20.0, 0.0);
    var expected = FindNearest(points, testPoint);
    var treeRoot = TSpatialSearch.Build(points);
    var nearest = treeRoot.FindNearest(testPoint);
    Assert.That(nearest, Is.EqualTo(expected));
  }

  [Test, Combinatorial]
  public void FindRange(
    [Values(1_000, 10_000, 100_000)] int numberOfPoints,
    [Values(0.1, 1.0, 10.0, 100.0)] double size,
    [Values(0, 1, 42)] int randomSeed)
  {
    var range = size * 0.01;
    var random = new Random(randomSeed);
    var points = GeneratePoints(numberOfPoints, size, random);
    var treeRoot = TSpatialSearch.Build(points);
    int iteration = 0;
    while (iteration++ < Iterations)
    {
      SimplePoint testPoint = Vector128.Create(
        random.NextDouble() * size,
        random.NextDouble() * size);
      var expectedValues = FindRange(points, testPoint, range);
      var values = treeRoot.FindInRadius(testPoint, range);
      Assert.That(values, Is.EquivalentTo(expectedValues), $"Failed at iteration {iteration}");
    }
  }

  private IEnumerable<(SimplePoint Point, double Distance)> FindRange(
    IEnumerable<SimplePoint> points, 
    SimplePoint testPoint,
    double range)
  {
    return points
      .Select(p => (p, VectorExtensions.Distance(p, testPoint)))
      .Where(p => p.Item2 <= range);
  }

  private static IEnumerable<SimplePoint> GeneratePoints(
    int numberOfPoints,
    double size,
    Random random)
  {
    return Enumerable
      .Range(0, numberOfPoints)
      .Select(_ => (SimplePoint)Vector128.Create(
        random.NextDouble() * size,
        random.NextDouble() * size))
      .ToList();
  }

  private static (SimplePoint p, double) FindNearest(
    IEnumerable<SimplePoint> points, 
    SimplePoint testPoint)
  {
    return points
      .Select(p => (p, VectorExtensions.Distance(p, testPoint)))
      .OrderBy(p => p.Item2).First();
  }
}
