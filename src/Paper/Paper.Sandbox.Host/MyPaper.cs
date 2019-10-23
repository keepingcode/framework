using System;
using System.Collections.Generic;
using System.Text;
using Paper.Design;
using Toolset;

namespace Paper.Sandbox.Host
{
  [Expose]
  [Paper(Module = "SampleModule", Schema = "SampleSchema")]
  class MyPaper : IPaper
  {
  }
}
