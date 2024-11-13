using System.Runtime.Intrinsics;

namespace QuadTree;

public static class QuadTree
{
  public static QuadTreeCell BuildQadTree(IEnumerable<Vector128<double>> points)
  {
    var maxX = points.Max(x => x[0]);
    var minX = points.Min(x => x[0]);
    var maxY = points.Max(x => x[1]);
    var minY = points.Min(x => x[1]);

    var center = Vector128.Create((maxX + minX) * 0.5, (maxY + minX) * 0.5);
    var size = Math.Max(maxX-minX,maxY-minY);

    var root = new QuadTreeCell(center, size);
    foreach (var point in points)
      root.AddPoint(point);
    return root;
  }
}
