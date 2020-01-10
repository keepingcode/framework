using Paper.Design;
using System;
using System.Collections.Generic;
using System.Text;
using Toolset;

namespace Paper.Sandbox.Host.Demo.Filters
{
  public class GroupFilter : IFilter
  {
    public Var<string> Name { get; set; }
  }
}
