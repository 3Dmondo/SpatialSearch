using System.Runtime.Intrinsics;

namespace QuadTree;

public static class QuadTree
{
  public static QuadTreeCell BuildQadTree(IEnumerable<Vector128<double>> points)
  {
    var max = points.Max();
    var min = points.Min();
    var center = (max + min) * 0.5;
    var size2 = max - min;
    var size = Math.Max(size2[0], size2[1]);
    var root = new QuadTreeCell(center, size);
    foreach (var point in points)
      root.AddPoint(point);
    return root;
  }
}
