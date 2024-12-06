using NUnit.Framework;
using SpatialSearch.Extensions;
using System.Runtime.Intrinsics;

namespace SpatialSearch.Tests;

[TestFixture]
public class GrandCircleDistance
{
  private SimplePoint point1 = (SimplePoint)Vector128.Create(12.474775403146737, 41.87365126992505);
  private SimplePoint point2 = (SimplePoint)Vector128.Create(11.233278064210367, 43.76315996157264);
  private double Expected = 233.22;

  [Test]
  public void GrandCircleDistanceTestPoint()
  {
    var distance = point1.GrandCircleDistance(point2);
    Assert.That(distance, Is.EqualTo(Expected).Within(0.005));
  }

  [Test]
  public void GrandCircleDistanceTestVector()
  {
    var distance = point1.ToVector128().GrandCircleDistance(point2);
    Assert.That(distance, Is.EqualTo(Expected).Within(0.005));
  }
}
