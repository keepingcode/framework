using System;
using Toolset.Collections;

namespace Paper.Media
{
  public interface IMedia : INode, IExtendedCollection<INode>
  {
    string Title { get; set; }
  }
}
