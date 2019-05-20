using System;
using System.Collections.Generic;
using System.Text;
using Toolset.Collections;

namespace Paper.Media3
{
  public class LinkCollection : Collection<ILink>
  {
    public LinkCollection()
    {
    }

    public LinkCollection(int capacity)
      : base(capacity)
    {
    }

    public LinkCollection(IEnumerable<ILink> items)
      : base(items)
    {
    }
  }
}
