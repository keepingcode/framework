using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Media
{
  public class Property : INode
  {
    public CaseVariantText Name { get; set; }

    public INode Value { get; set; }
  }
}
