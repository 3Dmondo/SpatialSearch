using BenchmarkDotNet.Attributes;
using SpatialSearch.Tests;
using System.Runtime.Intrinsics;
using SpatialSearch.Extensions;

namespace Benchmark;

[MemoryDiagnoser]
public class GrandCircleBenchmark
{
  private SimplePoint point1 = (SimplePoint)Vector128.Create(12.474775403146737, 41.87365126992505);
  private SimplePoint point2 = (SimplePoint)Vector128.Create(11.233278064210367, 43.76315996157264);
  private double Expected = 233.22;

  [Benchmark]
  public double GrandCircleDistancePoint()
  {
    return point1.GrandCircleDistance(point2);
  }
  [Benchmark]
  public double GrandCircleDistanceVector()
  {
    return VectorExtensions.GrandCircleDistance(point1.ToVector128(), point2.ToVector128());
  }
  [Benchmark]
  public double GrandCircleDistanceVectorNet9()
  {
    return GrandCircleComputer.GrandCircleDistance(point1.ToVector128(), point2.ToVector128());
  }
}
