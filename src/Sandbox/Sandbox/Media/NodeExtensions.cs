using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paper.Media
{
  public static class NodeExtensions
  {
    public static IEnumerable<INode> DescendantNodes(this INode node)
    {
      var nodes = node as INodeEnumerable;
      if (nodes == null)
        yield break;

      foreach (var child in nodes.SelectMany(x => x.DescendantNodesAndSelf()))
      {
        yield return child;
      }
    }

    public static IEnumerable<INode> DescendantNodesAndSelf(this INode node)
    {
      yield return node;

      var nodes = node as INodeEnumerable;
      if (nodes == null)
        yield break;

      foreach (var child in nodes.SelectMany(x => x.DescendantNodesAndSelf()))
      {
        yield return child;
      }
    }
  }
}
