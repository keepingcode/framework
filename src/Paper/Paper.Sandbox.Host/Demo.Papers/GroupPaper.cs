using Paper.Design;
using Paper.Sandbox.Host.Demo.Business;
using Paper.Sandbox.Host.Demo.Domain;
using Paper.Sandbox.Host.Demo.Filters;
using Paper.Sandbox.Host.Demo.Forms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolset;

namespace Paper.Sandbox.Host.Demo.Papers
{
  [Expose]
  public class GroupPaper : GroupBusiness, IPaper
  {
  }
}
