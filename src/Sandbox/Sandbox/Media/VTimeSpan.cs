using System;
using System.Collections.Generic;
using System.Text;
using Toolset;

namespace Paper.Media
{
  public class VTimeSpan : IValue
  {
    public VTimeSpan(TimeSpan timeSpan)
      => Value = timeSpan;

    public TimeSpan Value { get; }

    object IValue.Value
      => Value;

    public override string ToString()
      => Change.To<string>(Value);

    public override int GetHashCode()
      => Value.GetHashCode();

    public override bool Equals(object obj)
      => Value.Equals(obj);

    public static implicit operator TimeSpan(VTimeSpan time)
      => time.Value;

    public static implicit operator VTimeSpan(TimeSpan time)
      => new VTimeSpan(time);
  }
}
