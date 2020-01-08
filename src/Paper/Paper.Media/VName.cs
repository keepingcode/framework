using System;
using System.Collections.Generic;
using System.Text;
using Toolset;

namespace Paper.Media
{
  public class VName : VString
  {
    public VName(string value)
      : base(value, isCaseVariant: true)
    {
    }

    public override VString Clone()
      => new VName(Value);

    public static implicit operator string(VName text)
      => text.Value;

    public static implicit operator VName(string text)
      => new VName(text);
  }
}
