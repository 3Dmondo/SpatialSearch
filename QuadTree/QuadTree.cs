using QuadTree.Extensions;

namespace QuadTree;

public static class QuadTree
{
  public static QuadTreeCell<T> BuildQadTree<T>(IEnumerable<T> points)
    where T : IPoint
  {
    var max = points.MaxCoordinates();
    var min = points.MinCoordinates();
    var center = (max + min) * 0.5;
    var size2 = max - min;
    var size = Math.Max(size2[0], size2[1]);
    var root = new QuadTreeCell<T>(center, size);
    foreach (var point in points)
      root.AddPoint(point);
    return root;
  }
}
