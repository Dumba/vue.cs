using System;
using System.Collections.Generic;

namespace Vue.cs.Generator.Expansions
{
  public static class EnumerableExtension
  {
    public static IEnumerable<TItem> ForEach<TItem>(this IEnumerable<TItem> self, Action<TItem> action)
    {
      foreach (TItem item in self) {
        action(item);
      }

      return self;
    }
  }
}