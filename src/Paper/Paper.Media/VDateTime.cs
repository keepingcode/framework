using System;
using System.Collections.Generic;
using System.Text;
using Toolset;

namespace Paper.Media
{
  public class VDateTime : IValue
  {
    public VDateTime(DateTime dateTime)
      => Value = dateTime;

    public DateTime Value { get; }

    object IValue.Value
      => Value;

    public override string ToString()
      => Change.To<string>(Value);

    public override int GetHashCode()
      => Value.GetHashCode();

    public override bool Equals(object obj)
      => Value.Equals(obj);

    public static implicit operator DateTime(VDateTime time)
      => time.Value;

    public static implicit operator VDateTime(DateTime time)
      => new VDateTime(time);
  }
}
