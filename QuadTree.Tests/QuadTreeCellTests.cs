using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Intrinsics;

namespace QuadTree.Tests;

[TestClass]
public class QuadTreeCellTests
{

  [TestMethod]
  public void FindNearest()
  {
    var random = new Random(42);
    var points = Enumerable.Range(0, 1000).Select(_ => Vector128.Create(random.NextDouble(), random.NextDouble())).ToList();
    var testPoint = Vector128.Create(random.NextDouble(), random.NextDouble());
    var expected = points.OrderBy(p => VectorExtensions.DistanceSquared(p, testPoint)).First();
    var tree = QuadTree.BuildQadTree(points);
    var nearest = tree.FindNearest(testPoint);
    Assert.AreEqual(expected, nearest.Item1);
  } 
}
