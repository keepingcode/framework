using System;
using System.Collections.Generic;
using System.Text;
using Toolset;

namespace Paper.Media
{
  public class VLong : IValue
  {
    public VLong(long value)
      => Value = value;

    public long Value { get; }

    object IValue.Value => Value;

    public override string ToString()
      => Change.To<string>(Value);

    public override int GetHashCode()
      => Value.GetHashCode();

    public override bool Equals(object obj)
      => Value.Equals(obj);

    public static implicit operator long(VLong number)
      => number.Value;

    public static implicit operator VLong(long number)
      => new VLong(number);
  }
}
