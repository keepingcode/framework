using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Paper.Commons;
using Toolset.Collections;

namespace Paper.Rendering
{
  public class PipelineCatalog : Catalog<IPipeline>, IPipelineCatalog
  {
    public PipelineCatalog()
      : base(item => item.Route)
    {
    }
  }
}