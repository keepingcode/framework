using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Paper.Media
{
  public class NodeCollection<T> : Collection<T>, INode
    where T : INode
  {
  }
}
