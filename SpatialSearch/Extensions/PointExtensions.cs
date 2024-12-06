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

  public static int Median<T>(this Span<T> points, int axis) where T : IPoint
  {
    if (points == null || points.Length == 0)
      throw new ArgumentException("Points array cannot be null or empty.", nameof(points));

    int k = points.Length / 2;
    return QuickSelect(points, 0, points.Length - 1, k, axis);
  }

  private static int QuickSelect<T>(Span<T> points, int left, int right, int k, int axis) where T : IPoint
  {
    if (left == right)
      return left;

    int pivotIndex = Partition(points, left, right, axis);
    if (k == pivotIndex)
      return k;
    else if (k < pivotIndex)
      return QuickSelect(points, left, pivotIndex - 1, k, axis);
    else
      return QuickSelect(points, pivotIndex + 1, right, k, axis);
  }

  private static int Partition<T>(Span<T> points, int left, int right, int axis) where T : IPoint
  {
    var pivotValue = GetCoordinate(points[right], axis);
    int storeIndex = left;

    for (int i = left; i < right; i++)
    {
      if (GetCoordinate(points[i], axis) < pivotValue)
      {
        Swap(points, storeIndex, i);
        storeIndex++;
      }
    }
    Swap(points, storeIndex, right);
    return storeIndex;
  }

  private static double GetCoordinate<T>(T point, int axis) where T : IPoint
  {
    return axis == 0 ? point.X : point.Y;
  }

  private static void Swap<T>(Span<T> points, int i, int j)
  {
    T temp = points[i];
    points[i] = points[j];
    points[j] = temp;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static Vector128<double> ToVector128<T>(this T point) where T : IPoint
   => Vector128.Create(point.X, point.Y);


  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static Vector128<double> MaxCoordinates<T>(this IEnumerable<T> points)
    where T : IPoint
    => points.Aggregate(
         MinValue,
         (acc, point) => Vector128.Max(acc, point.ToVector128()));


  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static Vector128<double> MinCoordinates<T>(this IEnumerable<T> points)
    where T : IPoint
    => points.Aggregate(
      MaxValue,
      (acc, point) => Vector128.Min(acc, point.ToVector128()));


  private const double EarthRadiusKm = 6371.0;

  public static double ToRadians(this double degrees)
  {
    return degrees * (Math.PI / 180.0);
  }

  public static double GrandCircleDistance(this IPoint point1, IPoint point2)
  {
    double lat1 = point1.Y.ToRadians();
    double lon1 = point1.X.ToRadians();

    double lat2 = point2.Y.ToRadians();
    double lon2 = point2.X.ToRadians();

    double dlat = lat2 - lat1;
    double dlon = lon2 - lon1;

    double a = Math.Sin(dlat / 2) * Math.Sin(dlat / 2) +
               Math.Cos(lat1) * Math.Cos(lat2) *
               Math.Sin(dlon / 2) * Math.Sin(dlon / 2);

    double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

    return EarthRadiusKm * c;
  }
}
