using System.Runtime.Intrinsics;

namespace SpatialSearch.Tests;

public class PointsGenerator
{
  double Size;
  Random Random;

  public PointsGenerator(double size, int seed)
  {
    Size = size;
    Random = new Random(seed);
  }

  public IEnumerable<SimplePoint> GeneratePoints(
  int numberOfPoints)
  {
    return Enumerable
      .Range(0, numberOfPoints)
      .Select(_ => (SimplePoint)Vector128.Create(
        Random.NextDouble() * Size,
        Random.NextDouble() * Size))
      .ToList();
  }
}
