using SpatialSearch.Abstractions;
using SpatialSearch.Extensions;
using System.Runtime.Intrinsics;

namespace SpatialSearch;

public struct BoundingBox
{
  internal readonly Vector128<double> Min { get; }
  internal readonly Vector128<double> Max { get; }
  internal readonly Vector128<double> Center { get; }

  public BoundingBox(IPoint min, IPoint max)
    : this(min.ToVector128(), max.ToVector128()) { }

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

  internal BoundingBox GetChild(Vector128<double> greaterThanCenter)
  {
    var min = Vector128.ConditionalSelect(greaterThanCenter, Center, Min);
    var max = Vector128.ConditionalSelect(greaterThanCenter, Max, Center);
    return new BoundingBox(min, max);
  }

  internal (BoundingBox Left, BoundingBox Right) Split(int index, double value)
  {
    return
      (
        new BoundingBox(Min, Max.WithElement(index, value)),
        new BoundingBox(Min.WithElement(index, value), Max)
      );
  }
}
