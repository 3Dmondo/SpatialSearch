using System.Runtime.Intrinsics;

namespace QuadTree.Abstractions;

public interface IQuadTreeCell<T> where T : IPoint
{
  void AddPoint(T point);
  (T? Point, double DistanceSquared) FindNearest(Vector128<double> point, double minDistanceSquared = double.MaxValue);
}