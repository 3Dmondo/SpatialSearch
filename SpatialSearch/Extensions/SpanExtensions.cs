namespace SpatialSearch.Extensions;

internal static class SpanExtensions
{
  public static IEnumerable<TResult> Select<T,TResult>(this Span<T> span, Func<T, TResult> selector)
  {
    for (int i = 0; i < span.Length; i++)
    {
      yield return selector(span[i]);
    }
  }
}
