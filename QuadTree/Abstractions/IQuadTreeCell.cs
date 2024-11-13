namespace QuadTree.Abstractions;

public interface IQuadTreeCell<T> where T : IPoint
{
  void AddPoint(T point);
  (T? Point, double DistanceSquared) FindNearest(IPoint point, double minDistanceSquared = double.MaxValue);
}