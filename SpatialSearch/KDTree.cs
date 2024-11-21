using SpatialSearch.Abstractions;
using SpatialSearch.Extensions;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace SpatialSearch;

public class KDTree : ISpatialSearch
{
  public static ISpatialSearch<T> Build<T>(IEnumerable<T> points) where T : IPoint
  {
    var boundingBox = points.GetBoundingBox();
    return new KDTree<T>(points.ToArray().AsMemory(), boundingBox, 0);
  }
}

internal class KDTree<T> : ISpatialSearch<T> where T : IPoint
{
  private readonly KDTree<T>[] Children = new KDTree<T>[2];
  private readonly Memory<T> Points;
  private readonly int Depth;
  private readonly int Count;
  private Func<IPoint, double> CoordinateSelector;
  private double MinAxixValue;
  private double MaxAxisValue;
  private double Pivot;
  private BoundingBox BoundingBox;
  public KDTree(
    Memory<T> points,
    BoundingBox boundingBox,
    int depth)
  {
    Points = points;
    Depth = depth;
    Count = points.Length;
    CoordinateSelector = (Depth & 1) == 0 ? p => p.X : p => p.Y;
    BoundingBox = boundingBox;

    var span = points.Span;
    MinAxixValue = double.MaxValue;
    MaxAxisValue = double.MinValue;
    for (int i = 0; i < span.Length; i++)
    {
      var value = span[i];
      var coordinate = CoordinateSelector(value);
      MinAxixValue = Math.Min(MinAxixValue, coordinate);
      MaxAxisValue = Math.Max(MaxAxisValue, coordinate);
    }


    if (Count > 1)
    {
      var pivotIndex = span.Median(Depth & 1);
      Pivot = CoordinateSelector(span[pivotIndex]);

      var subBoundingBoxes = boundingBox.Split(Depth & 1, Pivot);

      Children[0] = new KDTree<T>(
        Points[..pivotIndex],
        subBoundingBoxes[0],
        depth + 1);
      Children[1] = new KDTree<T>(
        Points[pivotIndex..],
        subBoundingBoxes[1],
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

  public IEnumerable<(T Point, double Distance)> FindInRadius(IPoint point, double radius)
  {
    var stack = new Stack<KDTree<T>>();
    stack.Push(this);
    var vector = point.ToVector128();
    var circle = new Circle(vector, radius);
    while (stack.Count > 0)
    {
      var node = stack.Pop();
      if (!node.BoundingBox.Intersects(circle))
        continue;
      if (node.Count == 1)
      {
        var value = node.Points.Span[0];
        var distance = vector.Distance(value.ToVector128());
        if (distance < radius)
          yield return (value, distance);
      }
      else
      {
        if (circle.Contains(node.BoundingBox))
        {
          for (int i = 0; i < node.Points.Span.Length; i++)
          {
            var value = node.Points.Span[i];
            var distance = vector.Distance(value.ToVector128());
            if (distance < radius)
              yield return (value, distance);
          }
        }
        else
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
    {
      var value = Points.Span[0];
      return (value, value.ToVector128().Distance(point.ToVector128()));
    }

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

  public override string ToString() => $"{nameof(KDTree)}<{typeof(T).Name}> N: {Count}";

}
