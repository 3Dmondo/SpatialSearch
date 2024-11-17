using SpatialSearch.Abstractions;
using SpatialSearch.Extensions;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace SpatialSearch;

public class KDTree : ISpatialSearch
{
  public static ISpatialSearch<T> Build<T>(IEnumerable<T> points) where T : IPoint
    => new KDTree<T>(points.ToArray(), 0);
}

internal class KDTree<T> : ISpatialSearch<T> where T : IPoint
{
  private readonly KDTree<T>[] Children = new KDTree<T>[2];
  private readonly T[] Points;
  private readonly int Depth;
  private readonly int Count;
  private Func<IPoint, double> CoordinateSelector;
  private double MinAxixValue;
  private double MaxAxisValue;
  private double Pivot;
  public KDTree(
    T[] points,
    int depth)
  {
    Points = points;
    Depth = depth;
    Count = points.Length;
    CoordinateSelector = (Depth & 1) == 0 ? p => p.X : p => p.Y;

    (MinAxixValue, MaxAxisValue) = points
      .Select(p => CoordinateSelector(p))
      .Aggregate(
        (Min: double.MaxValue, Max: double.MinValue),
        (acc, v) => (Math.Min(acc.Min, v), Math.Max(acc.Max, v)));

    if (Count > 1)
    {
      Points = Points
        .OrderBy(p => CoordinateSelector(p))
        .ToArray();

      var pivotIndex = Points.Length / 2;
      Pivot = CoordinateSelector(Points[pivotIndex]);

      Children[0] = new KDTree<T>(
        Points[..pivotIndex],
        depth + 1);
      Children[1] = new KDTree<T>(
        Points[pivotIndex..],
        depth + 1);
    }
  }

  public (T Point, double Distance) FindNearest(IPoint point)
  {
    var result = FindNearest(point, double.MaxValue);
    return (result.Point!, result.Distance);
  }

  public bool TryFindNearest(IPoint point, double minDistance, out (T? Point, double Distance) nearest)
  {
    nearest = FindNearest(point, minDistance);
    if (nearest.Distance < minDistance)
      return true;
    nearest = (default, double.MaxValue);
    return false;
  }

  public IEnumerable<(T Point, double Distance)> FindRange(IPoint point, double radious)
  {
    var stack = new Stack<KDTree<T>>();
    stack.Push(this);
    var vector = point.ToVector128();
    while (stack.Count > 0)
    {
      var node = stack.Pop();
      if (node.Count == 1)
      {
        var distance = vector.Distance(node.Points[0].ToVector128());
        if (distance < radious)
          yield return (node.Points[0]!, distance);
      }
      else
      {
        var pointCoordinate = CoordinateSelector(point);
        if (pointCoordinate + radious > MinAxixValue ||
            pointCoordinate - radious < MaxAxisValue)
          foreach (var child in node.Children)
            if (null != child)
              stack.Push(child);
      }
    }
  }

  private (T? Point, double Distance) FindNearest(IPoint point, double minDistance)
  {
    var pointCoordinate = CoordinateSelector(point);

    if (pointCoordinate + minDistance < MinAxixValue ||
        pointCoordinate - minDistance > MaxAxisValue)
      return (default, double.MaxValue);

    if (Count == 1)
      return (Points[0], Points[0].ToVector128().Distance(point.ToVector128()));

    (T?, double) candidate = (default, minDistance);
    var childIndex = pointCoordinate < Pivot ? 0 : 1;
    candidate = UpdateCandidateIfCloser(Children[childIndex], point, candidate);
    candidate = UpdateCandidateIfCloser(Children[childIndex ^ 1], point, candidate);
    return candidate;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static (T? candidate, double mindistance) UpdateCandidateIfCloser(
    KDTree<T> child,
    IPoint point,
    (T?, double Distance) candidate)
  {
    var nextCandidate = child
      .FindNearest(point, candidate.Distance);
    if (nextCandidate.Distance < candidate.Distance)
      candidate = nextCandidate;
    return candidate;
  }

  public override string ToString() => Count.ToString();

}
