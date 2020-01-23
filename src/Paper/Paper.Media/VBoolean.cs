using System;
using System.Collections.Generic;
using System.Text;
using Toolset;

namespace Paper.Media
{
  public class VBoolean : IValue
  {
    public static readonly VBoolean True = new VBoolean(true);
    public static readonly VBoolean False = new VBoolean(false);

    private VBoolean(bool value)
      => this.Value = value;

    public bool Value { get; set; }

    public static VBoolean Create(bool value)
      => value ? True : False;

    object IValue.Value
      => Value;

    public override string ToString()
      => Change.To<string>(Value);

    public override bool Equals(object obj)
      => Value.Equals(obj);

    public override int GetHashCode()
      => Value.GetHashCode();

    public static implicit operator bool(VBoolean bit)
      => bit.Value;

    public static implicit operator VBoolean(bool bit)
      => bit ? True : False;
  }
}
