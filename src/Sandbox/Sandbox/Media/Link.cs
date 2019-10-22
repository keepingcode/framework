using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolset.Collections;

namespace Paper.Media
{
  public class Link : INode, INodeEnumerable
  {
    public Text Title { get; set; }

    public NodeCollection<Text> Rel { get; set; }

    public Text Href { get; set; }

    private IEnumerable<INode> Enumerate()
    {
      if (Title != null) yield return Title;
      if (Rel != null) yield return Rel;
      if (Href != null) yield return Href;
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
