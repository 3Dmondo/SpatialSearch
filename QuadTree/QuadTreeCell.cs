using System.Runtime.Intrinsics;
namespace QuadTree;
public class QuadTreeCell
{
  private QuadTreeCell[] Children { get; } = new QuadTreeCell[4];
  private static readonly Vector128<long> Base2Indices = Vector128.Create(1L, 2L);
  private static readonly Vector128<long> One = Vector128.Create(1.0, 1.0).AsInt64();
  private static readonly Vector128<long> MinusOne = Vector128.Create(-1.0, -1.0).AsInt64();

  private double RadiusSquared { get; init; }

  public Vector128<double> Center { get; init; }
  public double Size { get; init; }
  public int NumberOfPoints { get; private set; }
  public Vector128<double> InnerPoint { get; private set; }

  public QuadTreeCell(Vector128<double> center, double size)
  {
    Center = center;
    Size = size;
    RadiusSquared = Size * Size * 0.5;
  }

  public void AddPoint(Vector128<double> point)
  {
    NumberOfPoints++;
    if (NumberOfPoints == 1)
    {
      InnerPoint = point;
      return;
    }
    if (NumberOfPoints == 2)
      AddToChild(InnerPoint);
    AddToChild(point);
  }

  private void AddToChild(Vector128<double> point)
  {
    var gt = Vector128.GreaterThan(point, Center).AsInt64();
    var childIndex = Vector128.Sum(Base2Indices & gt);
    var child = Children[childIndex];
    if (null == child)
    {
      var direction = (One & gt).AsDouble() + (MinusOne & ~gt).AsDouble();
      var childPosition = Center + direction * Size * 0.25;
      child = new QuadTreeCell(childPosition, Size * 0.5);
      Children[childIndex] = child;
    }
    child.AddPoint(point);
  }

  public (Vector128<double>, double distanceSquared) FindNearest(Vector128<double> point, double minDistanceSquared = double.MaxValue)
  {
    var distanceFromCenter = point.DistanceSquared(Center);

    if (distanceFromCenter > minDistanceSquared + RadiusSquared)
      return (Vector128<double>.AllBitsSet, double.MaxValue);

    if (NumberOfPoints == 1)
      return (InnerPoint, point.DistanceSquared(InnerPoint));

    Vector128<double> candidate = Vector128<double>.AllBitsSet;

    var gt = Vector128.GreaterThan(point, Center).AsInt64();
    var childIndex = Vector128.Sum(Base2Indices & gt);
    var child = Children[childIndex];
    if (null != child)
    {
      var (nextCandidate, distanceSquared) = child.FindNearest(point, minDistanceSquared);
      if (distanceSquared < minDistanceSquared)
      {
        minDistanceSquared = distanceSquared;
        candidate = nextCandidate;
      }
    }

    for (int i = 0; i < 4; i++)
      if (i != childIndex && null != Children[i])
      {
        child = Children[i];
        var (nextCandidate, distanceSquared) = child.FindNearest(point, minDistanceSquared);
        if (distanceSquared < minDistanceSquared)
        {
          minDistanceSquared = distanceSquared;
          candidate = nextCandidate;
        }
      }

    return (candidate, minDistanceSquared);
  }
}
