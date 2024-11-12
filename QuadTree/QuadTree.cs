using System.Runtime.Intrinsics;

namespace QuadTree;

public static class QuadTree
{
  public static QuadTreeCell BuildQadTree(IEnumerable<Vector128<double>> points)
  {
    var center = Vector128<double>.Zero;
    double count = 0.0;
    foreach (var point in points)
    {
      center += point;
      count += 1.0;
    }
    center /= count;
    var maxDistance = points.Select(p => p.DistanceSquared(center)).Max();
    var root = new QuadTreeCell(center, maxDistance * 2.1);
    foreach (var point in points)
      root.AddPoint(point);
    return root;
  }
}
