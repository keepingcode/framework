using System;
using System.Collections.Generic;
using System.Text;
using Toolset.Collections;

namespace Paper.Media
{
  public class Entity : INode
  {
    public Text Title { get; set; }

    public NodeCollection<Text> Class { get; set; }

    public NodeCollection<Text> Rel { get; set; }

    public NodeCollection<Property> Properties { get; set; }

    public NodeCollection<Entity> Entities { get; set; }

    public NodeCollection<Link> Links { get; set; }
  }
}
