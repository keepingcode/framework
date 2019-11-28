using Paper.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Rendering.Design
{
  public interface INode<TRecord> : INode
  {
    new TRecord Record { get; set; }
  }
}
