using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using SpatialSearch.Abstractions;

namespace SpatialSearch.Extensions;

public static class PointExtensions
{
  static Vector128<double> MinValue = Vector128.Create(double.MinValue, double.MinValue);
  static Vector128<double> MaxValue = Vector128.Create(double.MaxValue, double.MaxValue);

  public static BoundingBox GetBoundingBox<T>(this IEnumerable<T> points)
    where T : IPoint
  {
    var max = points.MaxCoordinates();
    var min = points.MinCoordinates();
    return new BoundingBox(min, max);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static Vector128<double> ToVector128<T>(this T point) where T : IPoint
   => Vector128.Create(point.X, point.Y);

  internal static Vector128<double> MaxCoordinates<T>(this IEnumerable<T> points)
    where T : IPoint
    => points.Aggregate(
         MinValue,
         (acc, point) => Vector128.Max(acc, point.ToVector128()));

  internal static Vector128<double> MinCoordinates<T>(this IEnumerable<T> points)
    where T : IPoint
    => points.Aggregate(
      MaxValue,
      (acc, point) => Vector128.Min(acc, point.ToVector128()));
}
