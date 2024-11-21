using NUnit.Framework;
using SpatialSearch.Abstractions;
using System.Runtime.Intrinsics;

namespace SpatialSearch.Tests;

[TestFixture(typeof(QuadTree))]
[TestFixture(typeof(KDTree))]
public class ISpatialSearchTests<TSpatialSearch>
  where TSpatialSearch : ISpatialSearch
{
  private const int Iterations = 10;

  [Test, Combinatorial]
  public void FindNearest(
    [Values(1_000, 10_000, 100_000)] int numberOfPoints,
    [Values(0.1, 1.0, 10.0, 100.0)] double size,
    [Values(0, 1, 42)] int randomSeed)
  {
    var pointsGEnerator = new PointsGenerator(size, randomSeed);
    var points = pointsGEnerator.GeneratePoints(numberOfPoints);
    var linearSearch = BasicSearch.Build(points);
    var spatialSearch = TSpatialSearch.Build(points);
    var iteration = 0;
    foreach (var testPoint in pointsGEnerator.GeneratePoints(Iterations))
    {
      var expected = linearSearch.FindNearest(testPoint);
      var nearest = spatialSearch.FindNearest(testPoint);
      Assert.That(nearest, Is.EqualTo(expected), $"Failed at iteration {iteration++}");
    }
  }

  [Test]
  public void FindNearestMinDistance()
  {
    var pointsGenerator = new PointsGenerator(1.0, 42);
    var points = pointsGenerator.GeneratePoints(100);
    var testPoint = (SimplePoint)Vector128.Create(2.0, 0.0);
    var result = TSpatialSearch
      .Build(points)
      .TryFindNearest(testPoint, 1.0, out var nearest);
    Assert.That(result, Is.False);
    Assert.That(nearest, Is.EqualTo((default(SimplePoint), double.MaxValue)));
  }

  [Test]
  public void FindNearestFar()
  {
    var pointsGenerator = new PointsGenerator(1.0, 42);
    var points = pointsGenerator.GeneratePoints(100);
    var testPoint = (SimplePoint)Vector128.Create(20.0, 0.0);
    var expected = BasicSearch.Build(points).FindNearest(testPoint);
    var nearest = TSpatialSearch.Build(points).FindNearest(testPoint);
    Assert.That(nearest, Is.EqualTo(expected));
  }

  [Test, Combinatorial]
  public void FindInRadius(
    [Values(1_000, 10_000, 100_000)] int numberOfPoints,
    [Values(0.1, 1.0, 10.0, 100.0)] double size,
    [Values(0, 1, 42)] int randomSeed)
  {
    var range = 2.5 * size / Math.Sqrt(numberOfPoints);
    var pointsGenerator = new PointsGenerator(size, randomSeed);
    var points = pointsGenerator.GeneratePoints(numberOfPoints);
    var linearSearch = BasicSearch.Build(points);
    var spatialSearch  = TSpatialSearch.Build(points);
    int iteration = 0;
    foreach (var testPoint in pointsGenerator.GeneratePoints(Iterations))
    {
      var expectedValues = linearSearch.FindInRadius(testPoint, range);
      var values = spatialSearch.FindInRadius(testPoint, range);
      Assert.That(
        values,
        Is.EquivalentTo(expectedValues),
        $"Failed at iteration {iteration}");
      Assert.That(values.Count(), Is.AtLeast(5),
        $"Failed at iteration {iteration}");
      Assert.That(values.Count(), Is.AtMost(30),
        $"Failed at iteration {iteration}");
      iteration++;
    }
  }
}
