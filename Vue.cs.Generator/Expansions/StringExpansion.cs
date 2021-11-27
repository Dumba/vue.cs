namespace Vue.cs.Generator.Expansions
{
  public static class StringExpansion
  {
    public static string Cut(this string self, int? startIndex = null, int? endIndex = null)
    {
      if (startIndex == null)
        startIndex = 0;
      if (startIndex < 0)
        startIndex += self.Length;

      if (endIndex == null)
        return self.Substring(startIndex.Value);

      if (endIndex < 0)
        endIndex += self.Length;

      if (startIndex.Value > endIndex.Value)
        throw new System.ArgumentOutOfRangeException("End cannot be smaller than start!");

      return self.Substring(startIndex.Value, endIndex.Value - startIndex.Value);
    }

    public static int? IndexOfOrDefault(this string self, string value, int startIndex = 0)
    {
      var index = self.IndexOf(value, startIndex);
      if (index == -1)
        return null;

      return index;
    }
  }
}