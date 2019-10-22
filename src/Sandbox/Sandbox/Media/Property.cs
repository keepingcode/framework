using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolset.Collections;

namespace Paper.Media
{
  public class Property : INode, INodeEnumerable
  {
    public CaseVariantText Name { get; set; }

    public IValue Value { get; set; }

    private IEnumerable<INode> Enumerate()
    {
      yield return Name;
      yield return Value;
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
