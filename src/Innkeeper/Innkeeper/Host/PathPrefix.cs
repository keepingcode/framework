using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Toolset.Collections;

namespace Innkeeper.Host
{
  public class PathPrefix
  {
    private static readonly Regex Pattern = new Regex("^/(?:[^/]+/)*$");

    private string prefix;

    public override string ToString()
    {
      return prefix;
    }

    public override int GetHashCode()
    {
      return prefix.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      var other = (obj as PathPrefix)?.prefix ?? obj as string;
      return prefix.Equals(obj);
    }

    public static implicit operator PathPrefix(string path)
    {
      if (path == null) return null;

      if (!Pattern.IsMatch(path))
      {
        var tokens = path.Split('/', '\\').Cast<string>().NotNullOrEmpty();
        path = "/" + string.Concat(tokens.Select(x => x + "/"));
      }

      return new PathPrefix { prefix = path };
    }

    public static implicit operator string(PathPrefix path)
    {
      return path?.prefix;
    }
  }
}
