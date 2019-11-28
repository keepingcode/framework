using Paper.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Rendering.Design
{
  internal class Node<TRecord> : INode<TRecord>
  {
    public INode Parent { get; set; }

    public TRecord Record { get; set; }

    public Entity Entity { get; set; }

    object INode.Record
    {
      get => Record;
      set => Record = (TRecord)value;
    }
  }
}
