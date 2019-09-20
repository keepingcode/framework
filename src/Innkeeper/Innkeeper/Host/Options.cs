using System.Collections.Generic;

namespace Innkeeper.Host
{
  public class Options
  {
    public List<string> Prefixes { get; } = new List<string>();

    public Options AddPrefix(string prefix)
    {
      Prefixes.Add(prefix);
      return this;
    }
  }
}