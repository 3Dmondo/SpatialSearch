using BenchmarkDotNet.Attributes;
using SpatialSearch.Abstractions;
using SpatialSearch.Tests;
using SpatialSearch;
using System.Runtime.Intrinsics;

namespace Benchmark;

public interface ISearchBenchmark
{
  public abstract int N { get; set; }
  public abstract double RadiusFactor { get; set; }
  public abstract void GlobalSetup();
  public abstract Vector128<double> FindNearest();
  public abstract double FindInRadius();
  public abstract ISpatialSearch<SimplePoint> Build();
}

[MemoryDiagnoser]
[MarkdownExporterAttribute.GitHub]
//[GenericTypeArguments(typeof(BasicSearch))]
[GenericTypeArguments(typeof(QuadTree))]
[GenericTypeArguments(typeof(KDTree))]
[GenericTypeArguments(typeof(KDTree2))]
public class SearchBenchmark<T> : ISearchBenchmark where T : ISpatialSearch
{
  const int Iterations = 10;
  //[Params(1_000, 10_000, 100_000)]
  public  int N { get; set; } = 100_000;
  [Params(2.0, 4.0, 8.0)]
  public double RadiusFactor { get; set; }
  double Radius;

  private SimplePoint[]? Points;
  private SimplePoint[]? testPoints;
  private ISpatialSearch<SimplePoint>? SpatialSearch;

  [GlobalSetup]
  public void GlobalSetup()
  {
    var pointsGenerator = new PointsGenerator(1.0, 42);
    Radius = RadiusFactor / Math.Sqrt(N);
    Points = pointsGenerator.GeneratePoints(N).ToArray();
    testPoints = pointsGenerator.GeneratePoints(Iterations).ToArray();
    SpatialSearch = T.Build(Points);
  }

  [Benchmark(OperationsPerInvoke = Iterations)]
  public Vector128<double> FindNearest()
  {
    var result = Vector128<double>.Zero;
    for (int i = 0; i < Iterations; i++)
      result += SpatialSearch!.FindNearest(testPoints![i]).Item1;
    return result / Iterations;
  }

  [Benchmark(OperationsPerInvoke = Iterations)]
  public double FindInRadius()
  {
    var result = 0.0;
    for (int i = 0; i < Iterations; i++)
      result += SpatialSearch!.FindInRadius(testPoints![i], Radius).Count();
    return result / Iterations;
  }

  [Benchmark()]
  public ISpatialSearch<SimplePoint> Build()
  {
    return T.Build(Points!);
  }
}
