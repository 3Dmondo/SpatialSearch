using SpatialSearch.Abstractions;

namespace QuadTree.Abstractions;

public interface IQuadTreeCell<T> : INearestPointFinder<T> where T : IPoint
{
  void AddPoint(T point);
}