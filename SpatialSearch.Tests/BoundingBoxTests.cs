using NUnit.Framework;
using System.Runtime.Intrinsics;

namespace SpatialSearch.Tests;

[TestFixture]
public class BoundingBoxTests
{
  [Test]
  public void Contains_PointInside_ReturnsTrue()
  {
    var min = Vector128.Create(0.0, 0.0);
    var max = Vector128.Create(10.0, 10.0);
    var boundingBox = new BoundingBox(min, max);
    var point = Vector128.Create(5.0, 5.0);

    var result = boundingBox.Contains(point);

    Assert.That(result, Is.True);
  }

  [Test]
  public void Contains_PointOutside_ReturnsFalse()
  {
    var min = Vector128.Create(0.0, 0.0);
    var max = Vector128.Create(10.0, 10.0);
    var boundingBox = new BoundingBox(min, max);
    var point = Vector128.Create(15.0, 15.0);

    var result = boundingBox.Contains(point);

    Assert.That(result, Is.False);
  }

  [Test]
  public void Intersects_OverlappingBoundingBoxes_ReturnsTrue()
  {
    var min1 = Vector128.Create(0.0, 0.0);
    var max1 = Vector128.Create(10.0, 10.0);
    var boundingBox1 = new BoundingBox(min1, max1);

    var min2 = Vector128.Create(5.0, 5.0);
    var max2 = Vector128.Create(15.0, 15.0);
    var boundingBox2 = new BoundingBox(min2, max2);

    var result = boundingBox1.Intersects(boundingBox2);

    Assert.That(result, Is.True);
  }

  [Test]
  public void Intersects_NonOverlappingBoundingBoxes_ReturnsFalse()
  {
    var min1 = Vector128.Create(0.0, 0.0);
    var max1 = Vector128.Create(10.0, 10.0);
    var boundingBox1 = new BoundingBox(min1, max1);

    var min2 = Vector128.Create(15.0, 15.0);
    var max2 = Vector128.Create(25.0, 25.0);
    var boundingBox2 = new BoundingBox(min2, max2);

    var result = boundingBox1.Intersects(boundingBox2);

    Assert.That(result, Is.False);
  }

  [Test]
  public void Contains_BoundingBoxInside_ReturnsTrue()
  {
    var min1 = Vector128.Create(0.0, 0.0);
    var max1 = Vector128.Create(10.0, 10.0);
    var boundingBox1 = new BoundingBox(min1, max1);

    var min2 = Vector128.Create(2.0, 2.0);
    var max2 = Vector128.Create(8.0, 8.0);
    var boundingBox2 = new BoundingBox(min2, max2);

    var result = boundingBox1.Contains(boundingBox2);

    Assert.That(result, Is.True);
  }

  [Test]
  public void Contains_BoundingBoxOutside_ReturnsFalse()
  {
    var min1 = Vector128.Create(0.0, 0.0);
    var max1 = Vector128.Create(10.0, 10.0);
    var boundingBox1 = new BoundingBox(min1, max1);

    var min2 = Vector128.Create(15.0, 15.0);
    var max2 = Vector128.Create(25.0, 25.0);
    var boundingBox2 = new BoundingBox(min2, max2);

    var result = boundingBox1.Contains(boundingBox2);

    Assert.That(result, Is.False);
  }

  [Test]
  public void Split_BoundingBox_ReturnsFourSubBoxes()
  {
    var min = Vector128.Create(0.0, 0.0);
    var max = Vector128.Create(10.0, 10.0);
    var boundingBox = new BoundingBox(min, max);

    var subBoxes = boundingBox.Split();

    Assert.That(subBoxes.Length, Is.EqualTo(4));
    Assert.That(subBoxes[0].Contains(Vector128.Create(2.5, 2.5)), Is.True);
    Assert.That(subBoxes[1].Contains(Vector128.Create(7.5, 2.5)), Is.True);
    Assert.That(subBoxes[2].Contains(Vector128.Create(2.5, 7.5)), Is.True);
    Assert.That(subBoxes[3].Contains(Vector128.Create(7.5, 7.5)), Is.True);
  }

}
