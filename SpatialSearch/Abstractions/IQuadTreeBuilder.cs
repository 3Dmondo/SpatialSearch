namespace SpatialSearch.Abstractions;

public interface IQuadTreeBuilder
{
  QuadTreeCell<T> Build<T>(IEnumerable<T> points) where T : IPoint;
}