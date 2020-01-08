using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolset.Collections;

namespace Paper.Media
{
  public class VArray : Collection<IValue>, IValueCollection, ICloneable
  {
    public VArray()
    {
    }

    public VArray(int capacity)
      : base(capacity)
    {
    }

    public VArray(IEnumerable<IValue> items)
      : base(items)
    {
    }

    object IValue.Value
      => this;

    public VArray Clone()
      => new VArray(this.Select(x => (IValue)(x as ICloneable)?.Clone() ?? x));

    object ICloneable.Clone()
      => Clone();
  }
}
