using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolset.Collections;

namespace Paper.Media
{
  public class Property
  {
    public CaseVariantText Name { get; set; }

    public IValue Value { get; set; }
  }
}
