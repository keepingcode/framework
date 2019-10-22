﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Media
{
  public class Text : INode, IValue
  {
    public string Value { get; set; }

    object IValue.Value => Value;

    public override string ToString()
    {
      return Value;
    }

    public override int GetHashCode()
    {
      return Value?.GetHashCode() ?? base.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      return Value?.Equals(obj?.ToString()) ?? base.Equals(obj);
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