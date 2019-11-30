using System;
using System.Collections.Generic;
using System.Text;
using Toolset;

namespace Paper.Media
{
  public class Bit : IValue
  {
    public bool Value { get; set; }

    object IValue.Value => Value;

    public override string ToString()
    {
      return Value.ToString();
    }

    public override int GetHashCode()
    {
      return Value.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      if (obj == null) return false;
      if (Change.Try(obj, out bool bit))
      {
        return bit.Equals(Value);
      }
      return false;
    }

    public static implicit operator bool(Bit bit)
    {
      return bit.Value;
    }

    public static implicit operator Bit(bool bit)
    {
      return new Bit { Value = bit };
    }
  }
}
