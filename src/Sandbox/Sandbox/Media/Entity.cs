using System;
using Toolset.Collections;

namespace Paper.Media
{
  public class Entity : NodeCollection, INode, IEntity
  {
    public virtual string Title { get => Get<string>(); set => Set(value); }
  }
}
