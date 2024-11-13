using QuadTree.Abstractions;
using QuadTree.Extensions;

namespace QuadTree;

public class QuadTreeBuilder : IQuadTreeBuilder
{
  public static QuadTreeBuilder Instance { get; } = new QuadTreeBuilder();
  private QuadTreeBuilder() { }
  public QuadTreeCell<T> Build<T>(IEnumerable<T> points)
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
