﻿using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using QuadTree.Abstractions;
using QuadTree.Extensions;

namespace QuadTree;

public class QuadTreeCell<T> : IQuadTreeCell<T> where T : IPoint
{
  private static readonly Vector128<long> Base2Indices = Vector128.Create(1L, 2L);
  private static readonly Vector128<long> One = Vector128.Create(1.0, 1.0).AsInt64();
  private static readonly Vector128<long> MinusOne = Vector128.Create(-1.0, -1.0).AsInt64();

  private readonly QuadTreeCell<T>[] Children = new QuadTreeCell<T>[4];
  private readonly double RadiusSquared;
  private readonly Vector128<double> Center;
  private readonly double Size;

  private int NumberOfPoints;
  private T? InnerPoint;
  private Vector128<double> InnerPointVector;

  public QuadTreeCell(Vector128<double> center, double size)
  {
    Center = center;
    Size = size;
    RadiusSquared = Size * Size * 0.5;
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
      child = new QuadTreeCell<T>(childCenter, Size * 0.5);
      Children[childIndex] = child;
    }
    child.AddPoint(point);
  }

  public bool TryFindNearest(IPoint point, double minDistance, out (T? Point, double Distance) nearest)
  {
    var result = FindNearest(point.ToVector128(), minDistance * minDistance);
    var distance = Math.Sqrt(result.DistanceSquared);
    nearest = (result.Point, distance);
    return distance < minDistance;
  }

  public (T? Point, double Distance) FindNearest(IPoint point)
  {
    var result = FindNearest(point.ToVector128(), double.MaxValue);
    var distance = Math.Sqrt(result.DistanceSquared);
    return (result.Point, distance);
  }

  private (T? Point, double DistanceSquared) FindNearest(Vector128<double> point, double minDistanceSquared)
  {
    var distanceFromCenter = point.DistanceSquared(Center);

    if (distanceFromCenter > minDistanceSquared + RadiusSquared)
      return (default, double.MaxValue);

    if (NumberOfPoints == 1)
      return (InnerPoint, point.DistanceSquared(InnerPointVector));

    (T?, double) candidate = (default, minDistanceSquared);

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
  private static (T? candidate, double mindistanceSquared) UpdateCandidateIfCloser(
    QuadTreeCell<T> child,
    Vector128<double> point,
    (T?, double DistanceSquared) candidate)
  {
    var nextCandidate = child
      .FindNearest(point, candidate.DistanceSquared);
    if (nextCandidate.DistanceSquared < candidate.DistanceSquared)
      candidate = nextCandidate;
    return candidate;
  }
}
