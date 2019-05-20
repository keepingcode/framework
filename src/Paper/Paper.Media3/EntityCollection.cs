using System;
using System.Collections.Generic;
using System.Text;
using Toolset.Collections;

namespace Paper.Media3
{
  public class EntityCollection : Collection<IEntity>
  {
    public EntityCollection()
    {
    }

    public EntityCollection(int capacity)
      : base(capacity)
    {
    }

    public EntityCollection(IEnumerable<IEntity> items)
      : base(items)
    {
    }
  }
}