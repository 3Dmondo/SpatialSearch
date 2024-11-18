using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using SpatialSearch.Abstractions;

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

}
