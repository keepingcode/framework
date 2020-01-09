using System;
using System.Collections.Generic;
using System.Text;
using Toolset;

namespace Paper.Sandbox.Host.Filters
{
  public class UserFilter
  {
    public Var<string> Login { get; set; }

    public Var<string> Name { get; set; }

    public Var<bool> Enabled { get; set; }
  }
}
