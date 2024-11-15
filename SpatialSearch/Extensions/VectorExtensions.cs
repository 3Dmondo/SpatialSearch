using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using SpatialSearch.Abstractions;

namespace QuadTree.Extensions;

internal static class VectorExtensions
{
  static Vector128<double> MinValue = Vector128.Create(double.MinValue, double.MinValue);
  static Vector128<double> MaxValue = Vector128.Create(double.MaxValue, double.MaxValue);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static double DistanceSquared(this Vector128<double> a, Vector128<double> b)
  {
    var diff = Vector128.Subtract(a, b);
    return Vector128.Dot(diff, diff);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static double Distance(this Vector128<double> a, Vector128<double> b)
    => Math.Sqrt(DistanceSquared(a, b));

  public static Vector128<double> MaxCoordinates<T>(this IEnumerable<T> points)
    where T : IPoint
  {
    var result = MinValue;
    foreach (var point in points)
      result = Vector128.Max(result, point.ToVector128());
    return result;
  }

  public static Vector128<double> MinCoordinates<T>(this IEnumerable<T> points)
    where T : IPoint
  {
    var result = MaxValue;
    foreach (var point in points)
      result = Vector128.Min(result, point.ToVector128());
    return result;
  }
}
