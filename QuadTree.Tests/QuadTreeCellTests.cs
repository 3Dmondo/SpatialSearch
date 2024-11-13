using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuadTree.Extensions;
using System.Drawing;
using System.Runtime.Intrinsics;

namespace QuadTree.Tests;

[TestClass]
public class QuadTreeCellTests
{

  [DataTestMethod]
  [DataRow(1000, 0.1)]
  [DataRow(10000, 0.1)]
  [DataRow(100000, 0.1)]
  [DataRow(1000, 1.0)]
  [DataRow(10000, 1.0)]
  [DataRow(100000, 1.0)]
  [DataRow(1000, 10.0)]
  [DataRow(10000, 10.0)]
  [DataRow(100000, 10.0)]
  public void FindNearest(int numberOfPoints, double size)
  {
    var random = new Random(42);
    var points = Enumerable
      .Range(0, numberOfPoints)
      .Select(_ => (SimplePoint)Vector128.Create(
        random.NextDouble() * size,
        random.NextDouble() * size))
      .ToList();
    SimplePoint testPoint = Vector128.Create(
      random.NextDouble() * size,
      random.NextDouble() * size);
    var expected = points
      .Select(p => (p, Math.Sqrt(VectorExtensions.DistanceSquared(p, testPoint))))
      .OrderBy(p => p.Item2).First();
    var treeRoot = QuadTreeBuilder.Instance.Build(points);
    var nearest = treeRoot.FindNearest(testPoint);
    Assert.AreEqual(expected, nearest);
  }

  [TestMethod]
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
    var treeRoot = QuadTreeBuilder.Instance.Build(points);
    var result = treeRoot.TryFindNearest(testPoint, 1.0, out var nearest);
    Assert.IsFalse(result);
    Assert.AreEqual(default, nearest.Point);
  }

  [TestMethod]
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
    var treeRoot = QuadTreeBuilder.Instance.Build(points);
    var nearest = treeRoot.FindNearest(testPoint);
    Assert.AreEqual(expected, nearest);
  }
}
