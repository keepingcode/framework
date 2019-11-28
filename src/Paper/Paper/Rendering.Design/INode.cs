using Paper.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Rendering.Design
{
  public interface INode
  {
    INode Parent { get; set; }

    object Record { get; set; }

    Entity Entity { get; set; }
  }
}
