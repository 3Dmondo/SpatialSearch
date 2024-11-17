using SpatialSearch.Abstractions;
using SpatialSearch.Extensions;
using System.Runtime.Intrinsics;

namespace SpatialSearch;

public struct BoundingBox
{
  public Vector128<double> Min;
  public Vector128<double> Max;

  public BoundingBox(IPoint min, IPoint max)
  {
    Min = min.ToVector128();
    Max = max.ToVector128();
  }

  internal BoundingBox(Vector128<double> min, Vector128<double> max)
  {
    Min = min;
    Max = max;
  }

  internal bool Contains(Vector128<double> point)
  {
    return Vector128.GreaterThanAll(point, Min) &&
           Vector128.LessThanOrEqualAll(point, Max);
  }

  internal bool Contains(Circle circle)
  {
    var radiusVector = Vector128.Create(circle.Radius);
    var min = Min - radiusVector;
    var max = Max + radiusVector;
    return Vector128.GreaterThanOrEqualAll(circle.Center, min) &&
           Vector128.LessThanOrEqualAll(circle.Center, max);
  }

  public bool Intersects(BoundingBox other)
  {
    return Vector128.LessThanOrEqualAll(Min, other.Max) &&
           Vector128.GreaterThanAll(Max, other.Min);
  }

  public bool Intersects(Circle circle)
  {
    var radiusVector = Vector128.Create(circle.Radius);
    var min = circle.Center - radiusVector;
    var max = circle.Center + radiusVector;
    return Vector128.LessThanOrEqualAll(Min, max) &&
           Vector128.GreaterThanOrEqualAll(Max, min);
  }

  public bool Contains(BoundingBox other)
  {
    return Vector128.GreaterThanAll(other.Min, Min) &&
           Vector128.LessThanOrEqualAll(other.Max, Max);
  }

  internal BoundingBox[] Split()
  {
    var center = (Min + Max) * 0.5;
    return
      [
        new BoundingBox(Min, center),
        new BoundingBox(
          Vector128.Create(center.GetElement(0), Min.GetElement(1)),
          Vector128.Create(Max.GetElement(0), center.GetElement(1))),
        new BoundingBox(
          Vector128.Create(Min.GetElement(0), center.GetElement(1)),
          Vector128.Create(center.GetElement(0), Max.GetElement(1))),
        new BoundingBox(center, Max)
      ];
  }

  internal BoundingBox[] Split(int index, double value)
  {
    return
      [
        new BoundingBox(Min, Max.WithElement(index, value)),
        new BoundingBox(Min.WithElement(index, value), Max)
      ];
  }
}
