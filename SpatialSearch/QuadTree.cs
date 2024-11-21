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
    var root = new QuadTree<T>(points.GetBoundingBox());
    foreach (var point in points)
      root.AddPoint(point);
    return root;
  }
}

internal class QuadTree<T> : ISpatialSearch<T> where T : IPoint
{
  private static readonly Vector128<long> Base2Indices = Vector128.Create(1L, 2L);

  private readonly QuadTree<T>[] Children = new QuadTree<T>[4];
  private readonly BoundingBox BoundingBox;

  private int NumberOfPoints;
  private T? InnerPoint;
  private Vector128<double> InnerPointVector;

  internal QuadTree(BoundingBox boundingBox)
  {
    BoundingBox = boundingBox;
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
    var gtd = Vector128.GreaterThan(point.ToVector128(), BoundingBox.Center);
    var gtl = gtd.AsInt64();
    var childIndex = Vector128.Sum(Base2Indices & gtl);
    var child = Children[childIndex];
    if (null == child)
    {
      child = new QuadTree<T>(BoundingBox.GetChild(~gtd));
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
    var circle = new Circle(vector, radius);
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
        foreach (var child in node.Children)
          if (null != child)
            if (child.BoundingBox.Intersects(circle))
              stack.Push(child);
      }
    }
  }

  private (T? Point, double Distance) FindNearest(Vector128<double> point, double minDistance)
  {
    if (!BoundingBox.Intersects(new Circle(point, minDistance)))
      return (default, double.MaxValue);

    if (NumberOfPoints == 1)
      return (InnerPoint, point.Distance(InnerPointVector));

    (T?, double) candidate = (default, minDistance);

    var gt = Vector128.GreaterThan(point, BoundingBox.Center).AsInt64();
    var childIndex = Vector128.Sum(Base2Indices & gt);

    for (long i = 0; i < 4; i++)
    {
      var child = Children[i ^ childIndex];
      if (child != null)
        candidate = UpdateCandidateIfCloser(child, point, candidate);
    }

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

  public override string ToString() => $"{nameof(KDTree)}<{typeof(T).Name}> N: {NumberOfPoints}";

}
