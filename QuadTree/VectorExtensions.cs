using System.Runtime.Intrinsics;

namespace QuadTree
{
  public static class VectorExtensions
  {
    public static double DistanceSquared(this Vector128<double> a, Vector128<double> b)
    {
      var diff = Vector128.Subtract(a, b);
      return Vector128.Dot(diff, diff);
    }
  }
}
