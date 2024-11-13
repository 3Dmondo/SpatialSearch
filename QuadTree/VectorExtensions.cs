using System.Runtime.Intrinsics;

namespace QuadTree
{
  public static class VectorExtensions
  {
    static Vector128<double> MinValue = Vector128.Create(double.MinValue, double.MinValue);
    static Vector128<double> MaxValue = Vector128.Create(double.MaxValue, double.MaxValue);

    public static double DistanceSquared(this Vector128<double> a, Vector128<double> b)
    {
      var diff = Vector128.Subtract(a, b);
      return Vector128.Dot(diff, diff);
    }

    public static Vector128<double> Max(this IEnumerable<Vector128<double>> vectors)
    {
      var result = MinValue;
      foreach (var vector in vectors)
        result = Vector128.Max(result, vector);
      return result;
    }

    public static Vector128<double> Min(this IEnumerable<Vector128<double>> vectors)
    {
      var result = MaxValue;
      foreach (var vector in vectors)
        result = Vector128.Min(result, vector);
      return result;
    }
  }
}
