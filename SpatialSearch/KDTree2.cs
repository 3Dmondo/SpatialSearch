using SpatialSearch.Abstractions;
using SpatialSearch.Extensions;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace SpatialSearch;

public class KDTree2 : ISpatialSearch
{
  public static ISpatialSearch<T> Build<T>(IEnumerable<T> points) where T : IPoint
  {
    var boundingBox = points.GetBoundingBox();
    return new KDTree2<T>(points.ToArray().AsMemory(), boundingBox, 0);
  }
}

internal class KDTree2<T> : ISpatialSearch<T> where T : IPoint
{
  private readonly KDTree2<T>[] Children = new KDTree2<T>[2];
  private readonly Memory<T> Points;
  private readonly int Depth;
  private readonly int Count;
  private Func<IPoint, double> CoordinateSelector;
  private double MinAxixValue;
  private double MaxAxisValue;
  private double Pivot;
  private BoundingBox BoundingBox;
  private T? InnerPoint;
  private Vector128<double> InnerPointVector;
  public KDTree2(
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

      Children[0] = new KDTree2<T>(
        Points[..pivotIndex],
        subBoundingBoxes.Left,
        depth + 1);
      Children[1] = new KDTree2<T>(
        Points[pivotIndex..],
        subBoundingBoxes.Right,
        depth + 1);
    }
    if (Count == 1)
    {
      InnerPoint = Points.Span[0];
      InnerPointVector = InnerPoint.ToVector128();
    }
  }

  public (T Point, double Distance) FindNearest(IPoint point)
  {
    var result = FindNearest(point.ToVector128(), double.MaxValue);
    return (result.Point!, result.Distance);
  }

  public bool TryFindNearest(IPoint point, double minDistance, out (T? Point, double Distance) nearest)
  {
    nearest = FindNearest(point.ToVector128(), minDistance);
    if (nearest.Distance < minDistance)
      return true;
    nearest = (default, double.MaxValue);
    return false;
  }

  public IEnumerable<(T Point, double Distance)> FindInRadius(IPoint point, double radius)
  {
    var result = new List<(T Point, double Distance)>();
    var vector = point.ToVector128();
    var circle = new Circle(vector, radius);
    if (!BoundingBox.Intersects(circle))
      return result;
    var stack = new Stack<KDTree2<T>>();
    stack.Push(this);
    while (stack.Count > 0)
    {
      var node = stack.Pop();
      if (node.Count == 1)
      {
        if (circle.Contains(node.InnerPointVector, out var distance))
          result.Add((node.InnerPoint!, distance));
      }
      else
      {
        if (circle.Contains(node.BoundingBox))
        {
          foreach (var value in node.Points.ToArray())
          {
            var distance = value.ToVector128().Distance(vector);
            if (distance <= radius)
              result.Add((value, distance));
            else
              throw new UnreachableException();
          }
        }
        else
          foreach (var child in node.Children)
            if (null != child)
              if (child.BoundingBox.Intersects(circle))
                stack.Push(child);
      }
    }
    return result;
  }

  private (T? Point, double Distance) FindNearest(Vector128<double> point, double minDistance)
  {
    var pointCoordinate = point[Depth & 1];

    if (!BoundingBox.Intersects(new Circle(point, minDistance)))
      return (default, double.MaxValue);

    if (Count == 1)
    {
      var value = InnerPoint;
      return (value, InnerPointVector.Distance(point));
    }

    (T?, double) candidate = (default, minDistance);
    var childIndex = pointCoordinate < Pivot ? 0 : 1;
    candidate = UpdateCandidateIfCloser(Children[childIndex], point, candidate);
    candidate = UpdateCandidateIfCloser(Children[childIndex ^ 1], point, candidate);
    return candidate;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static (T? candidate, double mindistance) UpdateCandidateIfCloser(
    KDTree2<T> child,
    Vector128<double> point,
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
