using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using QuadTree.Abstractions;

namespace QuadTree.Extensions;

internal static class PointExtensions
{
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector128<double> ToVector128<T>(this T point) where T : IPoint
   => Vector128.Create(point.X, point.Y);
}
