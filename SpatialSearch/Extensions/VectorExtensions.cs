using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace SpatialSearch.Extensions;

internal static class VectorExtensions
{
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static double DistanceSquared(this Vector128<double> a, Vector128<double> b)
  {
    var diff = Vector128.Subtract(a, b);
    return Vector128.Dot(diff, diff);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static double Distance(this Vector128<double> a, Vector128<double> b)
    => Math.Sqrt(DistanceSquared(a, b));


  private const double EarthRadiusKm = 6371.0;
  public static double GrandCircleDistance(this Vector128<double> a, Vector128<double> b)
  {
    // [0] is longitude, [1] is latitude
    var aRadians = a.ToRadians();
    var bRadians = b.ToRadians();

    var lat1 = aRadians[1];
    var lat2 = bRadians[1];

    var halfDelta = (bRadians - aRadians) * 0.5;

    var sinDelta = Vector128.Create(Math.Sin(halfDelta[0]), Math.Sin(halfDelta[1]));

    var sinDeltaSquared = sinDelta * sinDelta;

    var c = sinDeltaSquared[1] +
            Math.Cos(lat1) * Math.Cos(lat2) *
            sinDeltaSquared[0];

    var d = 2.0 * Math.Atan2(Math.Sqrt(c), Math.Sqrt(1.0 - c));

    return EarthRadiusKm * d;
  }



  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static Vector128<double> ToRadians(this Vector128<double> LonLat)
  {
    return LonLat * Math.PI / 180;
  }
}

