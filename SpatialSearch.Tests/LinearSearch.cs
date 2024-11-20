﻿using SpatialSearch.Abstractions;
using SpatialSearch.Extensions;

namespace SpatialSearch.Tests;
public class LinearSearch : ISpatialSearch
{
  public static ISpatialSearch<T> Build<T>(IEnumerable<T> points) where T : IPoint
    => new LinearSearch<T>(points);
}

public class LinearSearch<T> : ISpatialSearch<T> where T : IPoint
{
  private IEnumerable<T> Points;
  public LinearSearch(IEnumerable<T> points)
  {
    Points = points;
  }

  public IEnumerable<(T Point, double Distance)> FindInRadius(IPoint point, double radius)
    => Points.Select(p => (
      p,
      p.ToVector128().Distance(point.ToVector128())))
    .Where(p => p.Item2 <= radius);

  public (T Point, double Distance) FindNearest(IPoint point)
    => Points.Select(p => (
      p,
      p.ToVector128().Distance(point.ToVector128())))
    .MinBy(p => p.Item2);

  public bool TryFindNearest(IPoint point, double minDistance, out (T? Point, double Distance) nearest)
  {
    nearest = FindNearest(point);
    if (nearest.Distance < minDistance)
      return true;
    else
    {
      nearest = (default, double.MaxValue);
      return false;
    }
  }
}