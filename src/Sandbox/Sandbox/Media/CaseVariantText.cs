using System;
using System.Collections.Generic;
using System.Text;
using Toolset;

namespace Paper.Media
{
  public class CaseVariantText : Text, INode, IPropertyValue
  {
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
