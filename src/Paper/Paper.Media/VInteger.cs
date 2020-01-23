using System;
using System.Collections.Generic;
using System.Text;
using Toolset;

namespace Paper.Media
{
  public class VInteger : IValue
  {
    public VInteger(int value)
      => Value = value;

    public int Value { get; set; }

    object IValue.Value => Value;

    public override string ToString()
      => Change.To<string>(Value);

    public override int GetHashCode()
      => Value.GetHashCode();

    public override bool Equals(object obj)
      => Value.Equals(obj);

    public static implicit operator int(VInteger number)
      => number.Value;

    public static implicit operator VInteger(int number)
      => new VInteger(number);
  }
}
