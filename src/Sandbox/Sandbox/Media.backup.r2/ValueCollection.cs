using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolset.Collections;

namespace Paper.Media
{
  public class ValueCollection : Collection<IValue>, IValueCollection
  {
    public ValueCollection()
    {
    }

    public ValueCollection(int capacity)
      : base(capacity)
    {
    }

    public ValueCollection(IEnumerable<IValue> items)
      : base(items)
    {
    }

    object IValue.Value => this;

    public ValueCollection Clone() => new ValueCollection(this.Select(x => x.Clone()));

    IValue IValue.Clone() => Clone();

    object ICloneable.Clone() => Clone();
  }
}
