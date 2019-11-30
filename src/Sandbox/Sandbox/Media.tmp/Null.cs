using System;
using System.Collections.Generic;
using System.Text;
using Toolset;

namespace Paper.Media
{
  public class Null : IValue
  {
    public static Null Default = new Null();

    private Null()
    {
    }

    object IValue.Value => null;

    public override string ToString()
    {
      return null;
    }
  }
}
