using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolset.Collections;

namespace Paper.Media
{
  public class NodeCollection<T> : Collection<T>, INode, INodeEnumerable
    where T : INode
  {
    public NodeCollection()
    {
    }

    public NodeCollection(int capacity)
      : base(capacity)
    {
    }

    public NodeCollection(IEnumerable<T> items)
      : base(items)
    {
    }

    IEnumerator<INode> IEnumerable<INode>.GetEnumerator()
    {
      return this.OfType<INode>().GetEnumerator();
    }
  }
}
