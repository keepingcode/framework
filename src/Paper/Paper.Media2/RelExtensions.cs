using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Media2
{
  public static class RelExtensions
  {
    public static string GetName(this Rel rel)
    {
      return rel.ToString().ToLower();
    }
  }
}
