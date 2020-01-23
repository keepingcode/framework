using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Media
{
  public class VString : IValue
  {
    public VString(string value, bool isCaseVariant = false)
    {
      if (value == null)
        throw new NullReferenceException("O texto não deve ser nulo.");

      this.Value = value;
      this.IsCaseVariant = isCaseVariant;
    }

    public virtual string Value { get; set; }

    public virtual bool IsCaseVariant { get; set; }

    object IValue.Value
      => Value;

    public override string ToString()
      => Value;

    public override int GetHashCode()
      => Value?.GetHashCode() ?? base.GetHashCode();

    public override bool Equals(object obj)
      => Value.Equals(obj);

    public virtual VString Clone()
      => new VString(Value);

    public static implicit operator string(VString text)
      => text.Value;

    public static implicit operator VString(string text)
      => new VString(text);
  }
}
