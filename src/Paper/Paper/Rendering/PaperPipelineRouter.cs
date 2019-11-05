using Innkeeper.Host;
using System;
using System.Collections.Generic;
using System.Text;
using Toolset;

namespace Paper.Rendering
{
  [Expose]
  class PaperPipelineRouter : IPipelineRouter
  {
    private IWebApp webApp;

    public PaperPipelineRouter(IWebApp webApp)
    {
      this.webApp = webApp;
    }

    public void Map(IRouter map)
    {
      map.Map<PaperPipeline>($"/Paper/Api/1/Catalogs/{webApp.Name}");
    }
  }
}
