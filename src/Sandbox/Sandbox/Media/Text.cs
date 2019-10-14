using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Media
{
  public class Text : INode, IPropertyValue
  {
    public string Value { get; set; }

    public override string ToString()
    {
      return Value;
    }

    public static implicit operator string(Text text)
    {
      return text.Value;
    }

    public static implicit operator Text(string text)
    {
      return new Text { Value = text };
    }
  }
}
