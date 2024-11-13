using System.Runtime.Intrinsics;

namespace QuadTree;

public interface IPoint
{
  Vector128<double> Point { get; }
}
