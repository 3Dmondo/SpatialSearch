using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using SpatialSearch.Abstractions;
using SpatialSearch.Extensions;

namespace SpatialSearch;

public class QuadTree : ISpatialSearch
{
  public static ISpatialSearch<T> Build<T>(IEnumerable<T> points)
  where T : IPoint
  {
    var max = points.MaxCoordinates();
    var min = points.MinCoordinates();
    var center = (max + min) * 0.5;
    var size2 = max - min;
    var size = Math.Max(size2[0], size2[1]);
    var root = new QuadTree<T>(center, size);
    foreach (var point in points)
      root.AddPoint(point);
    return root;
  }
}

internal class QuadTree<T> : ISpatialSearch<T> where T : IPoint
{
  private static readonly Vector128<long> Base2Indices = Vector128.Create(1L, 2L);
  private static readonly Vector128<long> One = Vector128.Create(1.0, 1.0).AsInt64();
  private static readonly Vector128<long> MinusOne = Vector128.Create(-1.0, -1.0).AsInt64();

  private readonly QuadTree<T>[] Children = new QuadTree<T>[4];
  private readonly double Radius;
  private readonly Vector128<double> Center;
  private readonly double Size;

  private int NumberOfPoints;
  private T? InnerPoint;
  private Vector128<double> InnerPointVector;

  internal QuadTree(Vector128<double> center, double size)
  {
    Center = center;
    Size = size;
    Radius = Math.Sqrt(Size * Size * 0.5);
  }

  public void AddPoint(T point)
  {
    NumberOfPoints++;
    if (NumberOfPoints == 1)
    {
      InnerPoint = point;
      InnerPointVector = InnerPoint.ToVector128();
      return;
    }
    if (NumberOfPoints == 2)
      AddToChild(InnerPoint!);
    AddToChild(point);
  }

  private void AddToChild(T point)
  {
    var gt = Vector128.GreaterThan(point.ToVector128(), Center).AsInt64();
    var childIndex = Vector128.Sum(Base2Indices & gt);
    var child = Children[childIndex];
    if (null == child)
    {
      var direction = (One & gt).AsDouble() + (MinusOne & ~gt).AsDouble();
      var childCenter = Center + direction * Size * 0.25;
      child = new QuadTree<T>(childCenter, Size * 0.5);
      Children[childIndex] = child;
    }
    child.AddPoint(point);
  }

  public bool TryFindNearest(IPoint point, double minDistance, out (T? Point, double Distance) nearest)
  {
    nearest = FindNearest(point.ToVector128(), minDistance);
    if (nearest.Distance < minDistance)
      return true;
    nearest = (default, double.MaxValue);
    return false;
  }

  public (T Point, double Distance) FindNearest(IPoint point)
  {
    var result = FindNearest(point.ToVector128(), double.MaxValue);
    return (result.Point!, result.Distance);
  }

  public IEnumerable<(T Point, double Distance)> FindInRadius(IPoint point, double radius)
  {
    var stack = new Stack<QuadTree<T>>();
    stack.Push(this);
    var vector = point.ToVector128();
    while (stack.Count > 0)
    {
      var node = stack.Pop();
      if (node.NumberOfPoints == 1)
      {
        var distance = vector.Distance(node.InnerPointVector);
        if (distance < radius)
          yield return (node.InnerPoint!, distance);
      }
      else
      {
        var distance = vector.Distance(node.Center);
        if (distance < radius + node.Radius)
          foreach (var child in node.Children)
            if (null != child)
              stack.Push(child);
      }
    }
  }

  private (T? Point, double Distance) FindNearest(Vector128<double> point, double minDistance)
  {
    var distanceFromCenter = point.DistanceSquared(Center);
    var maxDistanceSquared = minDistance + Radius;
    maxDistanceSquared *= maxDistanceSquared;

    if (distanceFromCenter > maxDistanceSquared)
      return (default, double.MaxValue);

    if (NumberOfPoints == 1)
      return (InnerPoint, point.Distance(InnerPointVector));

    (T?, double) candidate = (default, minDistance);

    var gt = Vector128.GreaterThan(point, Center).AsInt64();
    var childIndex = Vector128.Sum(Base2Indices & gt);
    var child = Children[childIndex];
    if (null != child)
      candidate = UpdateCandidateIfCloser(child, point, candidate);

    long nextChildIndex;

    nextChildIndex = childIndex ^ 1L;
    if (null != Children[nextChildIndex])
      candidate = UpdateCandidateIfCloser(Children[nextChildIndex], point, candidate);

    nextChildIndex = childIndex ^ 2L;
    if (null != Children[nextChildIndex])
      candidate = UpdateCandidateIfCloser(Children[nextChildIndex], point, candidate);

    nextChildIndex = childIndex ^ 3L;
    if (null != Children[nextChildIndex])
      candidate = UpdateCandidateIfCloser(Children[nextChildIndex], point, candidate);

    return candidate;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static (T? candidate, double mindistance) UpdateCandidateIfCloser(
    QuadTree<T> child,
    Vector128<double> point,
    (T?, double Distance) candidate)
  {
    var nextCandidate = child
      .FindNearest(point, candidate.Distance);
    if (nextCandidate.Distance < candidate.Distance)
      candidate = nextCandidate;
    return candidate;
  }
}
