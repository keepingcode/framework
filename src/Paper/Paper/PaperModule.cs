using Innkeeper.Host;
using Paper.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using Toolset;

namespace Paper
{
  [Expose]
  class PaperModule : IModule
  {
    public void Install(IObjectFactoryBuilder builder)
    {
      var objectFactory = builder.BuildObjectFactory();

      var paperCatalog = new AggregateCatalog();

      var exposedCatalog = objectFactory.CreateObject<ExposedPaperCatalog>();
      paperCatalog.AddCatalog(exposedCatalog);

      foreach (var type in ExposedTypes.GetTypes<IPaperCatalog>())
      {
        try
        {
          var catalog = (IPaperCatalog)objectFactory.CreateObject(type);
          paperCatalog.AddCatalog(catalog);
        }
        catch (Exception ex)
        {
          ex.Trace();
        }
      }

      builder.AddSingleton<IPaperCatalog>(paperCatalog);
    }
  }
}
