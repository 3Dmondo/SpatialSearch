using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace Benchmark;

public static class GrandCircleComputer
{
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static Vector128<double> ToRadians(this Vector128<double> LonLat)
  {
    return LonLat * Math.PI / 180;
  }
  private const double EarthRadiusKm = 6371.0;
  public static double GrandCircleDistance(this Vector128<double> a, Vector128<double> b)
  {
    // [0] is longitude, [1] is latitude
    var aRadians = a.ToRadians();
    var bRadians = b.ToRadians();

    var latitudes = Vector128.Create(aRadians[1], bRadians[1]);

    var halfDelta = (bRadians - aRadians) * 0.5;
    var sinHalfDelta = Vector128.Sin(halfDelta);
    var cosLatitudes = Vector128.Cos(latitudes);

    var sinDeltaSquared = sinHalfDelta * sinHalfDelta;

    var c = double.FusedMultiplyAdd(cosLatitudes[0] * cosLatitudes[1], sinDeltaSquared[0], sinDeltaSquared[1]);

    var d = 2.0 * Math.Atan2(Math.Sqrt(c), Math.Sqrt(1.0 - c));

    return EarthRadiusKm * d;
  }
}
