using SpatialSearch.Abstractions;
using SpatialSearch.Extensions;
using System.Runtime.Intrinsics;

namespace SpatialSearch;

public struct BoundingBox
{
  public Vector128<double> Min { get; }
  public Vector128<double> Max { get; }
  public Vector128<double> Center { get; }

  public BoundingBox(IPoint min, IPoint max)
  {
    Min = min.ToVector128();
    Max = max.ToVector128();
    Center = (Min + Max) * 0.5;
  }

  internal BoundingBox(Vector128<double> min, Vector128<double> max)
  {
    Min = min;
    Max = max;
    Center = (Min + Max) * 0.5;
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
    var p = Vector128.Max(Min, Vector128.Min(circle.Center, Max));
    var distanceSquared = p.DistanceSquared(circle.Center);
    return distanceSquared <= circle.Radius * circle.Radius;
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

  internal BoundingBox GetChild(long i)
  {
    return i switch
    {
      0 => new BoundingBox(
        Min,
        Center),
      1 => new BoundingBox(
        Min.WithElement(0, Center.GetElement(0)),
        Max.WithElement(1, Center.GetElement(1))),
      2 => new BoundingBox(
        Min.WithElement(1, Center.GetElement(1)),
        Max.WithElement(0, Center.GetElement(0))),
      3 => new BoundingBox(
        Center,
        Max),
      _ => throw new ArgumentOutOfRangeException(nameof(i))
    };
  }

  internal BoundingBox GetChild(Vector128<double> greaterThanCenter)
  {
    var min = Vector128.ConditionalSelect(greaterThanCenter, Min, Center);
    var max = Vector128.ConditionalSelect(greaterThanCenter, Center, Max);
    return new BoundingBox(min, max);
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
