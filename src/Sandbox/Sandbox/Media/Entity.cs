using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolset.Collections;

namespace Paper.Media
{
  public class Entity : INode, INodeEnumerable
  {
    public Text Title { get; set; }

    public NodeCollection<Text> Class { get; set; }

    public NodeCollection<Text> Rel { get; set; }

    public NodeCollection<Property> Properties { get; set; }

    public NodeCollection<Entity> Entities { get; set; }

    public NodeCollection<Link> Links { get; set; }

    private IEnumerable<INode> Enumerate()
    {
      if (Title != null) yield return Title;
      if (Class != null) yield return Class;
      if (Rel != null) yield return Rel;
      if (Properties != null) foreach (var child in Properties) yield return child;
      if (Entities != null) foreach (var child in Entities) yield return child;
      if (Links != null) foreach (var child in Links) yield return child;
    }

    public IEnumerator<INode> GetEnumerator()
    {
      return Enumerate().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return Enumerate().GetEnumerator();
    }
  }
}
