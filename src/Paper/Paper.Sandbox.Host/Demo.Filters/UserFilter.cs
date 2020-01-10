using Paper.Design;
using System;
using System.Collections.Generic;
using System.Text;
using Toolset;

namespace Paper.Sandbox.Host.Demo.Filters
{
  public class UserFilter : IFilter
  {
    public Var<string> Login { get; set; }
    public Var<string> Name { get; set; }
    public Var<bool> Enabled { get; set; }
  }
}
