using System;
using System.Collections.Generic;
using System.Text;
using Toolset.Collections;

namespace Paper.Media
{
  public class ValueCollection : Collection<IValue>, IValue
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
  }
}
