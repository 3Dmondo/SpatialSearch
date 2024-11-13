using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    var testPoint = Vector128.Create(
      random.NextDouble() * size,
      random.NextDouble() * size);
    var expected = points.OrderBy(p => VectorExtensions.DistanceSquared(p, testPoint)).First();
    var tree = QuadTree.BuildQadTree(points);
    var nearest = tree.FindNearest(testPoint);
    Assert.AreEqual(expected, nearest.Item1);
  }
}
