using System;
using System.Collections.Generic;
using System.Text;
using Toolset;

namespace Paper.Media
{
  public class CaseVariantText : Text
  {
    public override Text Clone() => new CaseVariantText { Value = Value };

    public static implicit operator string(CaseVariantText text)
    {
      return text.Value;
    }

    public static implicit operator CaseVariantText(string text)
    {
      return new CaseVariantText { Value = text };
    }
  }
}
