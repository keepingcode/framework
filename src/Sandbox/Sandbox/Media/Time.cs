using System;
using System.Collections.Generic;
using System.Text;
using Toolset;

namespace Paper.Media
{
  public class Time : INode, IValue
  {
    public DateTime Value { get; set; }

    object IValue.Value => Value;

    public override string ToString()
    {
      return Change.To<string>(Value);
    }

    public override int GetHashCode()
    {
      return Value.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      if (obj == null) return false;
      if (Change.Try(obj, out DateTime time))
      {
        return time.Equals(Value);
      }
      return false;
    }

    public static implicit operator DateTime(Time time)
    {
      return time.Value;
    }

    public static implicit operator Time(DateTime time)
    {
      return new Time { Value = time };
    }
  }
}
