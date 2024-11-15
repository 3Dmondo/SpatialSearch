namespace SpatialSearch.Abstractions;

public interface INearestPointFinder
{
  abstract static INearestPointFinder<T> Build<T>(IEnumerable<T> points) where T : IPoint;
}

/// <summary>
/// Interface for finding the nearest point to a given point in a set of points in two dimensions.
/// </summary>
/// <typeparam name="T">The type of the points in the set.</typeparam>
public interface INearestPointFinder<T>
{

  /// <summary>
  /// Finds the nearest point to the specified point.
  /// </summary>
  /// <param name="point">The point to which the nearest point is to be found.</param>
  /// <returns>A tuple containing the nearest point and the distance to it.</returns>
  (T Point, double Distance) FindNearest(IPoint point);

  /// <summary>
  /// Tries to find the nearest point to the specified point within a minimum distance.
  /// </summary>
  /// <param name="point">The point to which the nearest point is to be found.</param>
  /// <param name="minDistance">The minimum distance within which the nearest point should be found.</param>
  /// <param name="nearest">A tuple containing the nearest point and the distance to it if found; otherwise, the point is null and the distance is double.MaxValue.</param>
  /// <returns>True if a point is found within the minimum distance; otherwise, false.</returns>
  bool TryFindNearest(IPoint point, double minDistance, out (T? Point, double Distance) nearest);
}
