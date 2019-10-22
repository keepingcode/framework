using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Media
{
  public interface INodeEnumerable : IEnumerable<INode>, INode
  {
  }
}
