using SpatialSearch.Abstractions;
using SpatialSearch.Extensions;
using System.Runtime.Intrinsics;

namespace SpatialSearch;

public struct Circle
{
  internal Vector128<double> Center;
  internal double Radius;

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
    var minMax = Vector128.Create(boundingBox.Min.GetElement(0), boundingBox.Max.GetElement(1));
    var maxMin = Vector128.Create(boundingBox.Max.GetElement(0), boundingBox.Min.GetElement(1));
    return boundingBox.Min.Distance(Center) <= Radius &&
           boundingBox.Max.Distance(Center) <= Radius &&
           minMax.Distance(Center) <= Radius &&
           maxMin.Distance(Center) < Radius;
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
