using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolset.Collections;

namespace Paper.Media
{
  public class Property : ICloneable
  {
    public string Name { get; set; }

    public IValue Value { get; set; }

    public Property Clone()
    {
      return new Property
      {
        Name = Name,
        Value = (Value is ICloneable cloneable) ? (IValue)cloneable.Clone() : Value
      };
    }

    object ICloneable.Clone() => Clone();
  }
}
