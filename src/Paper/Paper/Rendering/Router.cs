using Innkeeper.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Toolset;

namespace Paper.Rendering
{
  [Expose]
  class Router : IPipelineRouter
  {
    private IWebApp webApp;

    public Router(IWebApp webApp)
    {
      this.webApp = webApp;
    }

    public void Map(IRouter map)
    {
      var catalog = webApp.Name;
      map.Map<CatalogPipeline>($"/Paper/Api/Catalogs/{catalog}");
      map.Map<PaperPipeline>($"/Paper/{catalog}");
    }
  }
}