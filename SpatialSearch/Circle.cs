using SpatialSearch.Abstractions;
using SpatialSearch.Extensions;
using System.Runtime.Intrinsics;

namespace SpatialSearch;

public struct Circle
{
  internal readonly Vector128<double> Center;
  internal readonly double Radius;

  public Circle(IPoint center, double radius)
  {
    Center = center.ToVector128();
    Radius = radius;
  }

  internal Circle(Vector128<double> center, double radius)
  {
    Center = center;
    Radius = radius;
  }

  public bool Contains(IPoint point, out double distance)
  {
    distance = Center.Distance(point.ToVector128());
    return distance <= Radius;
  }

  public bool Contains(BoundingBox boundingBox)
  {
    var gt= Vector128.GreaterThan(Center, boundingBox.Center);
    var furthestCorner = Vector128.ConditionalSelect(gt, boundingBox.Min, boundingBox.Max);
    var distanceSquared = Center.DistanceSquared(furthestCorner);
    return distanceSquared <= Radius * Radius;
  }

  public bool Intersects(BoundingBox boundingBox)
  {
    return boundingBox.Intersects(this);
  }

  public bool Intersects(Circle other)
  {
    var distance = Center.Distance(other.Center);
    return distance <= Radius + other.Radius;
  } 
}
