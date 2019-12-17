using System;
using Toolset.Collections;

namespace Paper.Media
{
  public class Entity : NodeCollection, INode, IEntity
  {
    public string Title
    {
      get => Get<string>();
      set => Set(value);
    }
  }
}
