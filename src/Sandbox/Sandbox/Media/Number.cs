using System;
using System.Collections.Generic;
using System.Text;
using Toolset;

namespace Paper.Media
{
  public class Number : INode, IValue
  {
    public decimal Value { get; set; }

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
      if (Change.Try(obj, out decimal number))
      {
        return number.Equals(Value);
      }
      return false;
    }

    public static implicit operator decimal(Number number)
    {
      return number.Value;
    }

    public static implicit operator Number(decimal number)
    {
      return new Number { Value = number };
    }

    public static implicit operator int(Number number)
    {
      return Change.To<int>(number.Value);
    }

    public static implicit operator Number(int number)
    {
      return new Number { Value = number };
    }

    public static implicit operator double(Number number)
    {
      return Change.To<double>(number.Value);
    }

    public static implicit operator Number(double number)
    {
      return new Number { Value = Change.To<decimal>(number) };
    }

    public static implicit operator float(Number number)
    {
      return Change.To<float>(number.Value);
    }

    public static implicit operator Number(float number)
    {
      return new Number { Value = Change.To<decimal>(number) };
    }
  }
}
