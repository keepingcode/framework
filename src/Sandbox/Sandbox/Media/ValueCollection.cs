using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Media
{
  public class ValueCollection : NodeCollection<IValue>, IValue, INode, INodeEnumerable
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
