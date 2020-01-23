using System;
using System.Collections.Generic;
using System.Text;
using Toolset;

namespace Paper.Media
{
  public class VDecimal : IValue
  {
    public VDecimal(decimal value)
      => Value = value;

    public VDecimal(double value)
      => Value = (decimal)value;

    public VDecimal(float value)
      => Value = (decimal)value;

    public decimal Value { get; set; }

    object IValue.Value
      => Value;

    public override string ToString()
      => Change.To<string>(Value);

    public override int GetHashCode()
      => Value.GetHashCode();

    public override bool Equals(object obj)
      => Value.Equals(obj);

    public static implicit operator decimal(VDecimal number)
      => number.Value;

    public static implicit operator VDecimal(decimal number)
      => new VDecimal(number);

    public static implicit operator double(VDecimal number)
      => (double)number.Value;

    public static implicit operator VDecimal(double number)
      => new VDecimal((decimal)number);

    public static implicit operator float(VDecimal number)
      => (float)number.Value;

    public static implicit operator VDecimal(float number)
      => new VDecimal((decimal)number);
  }
}
