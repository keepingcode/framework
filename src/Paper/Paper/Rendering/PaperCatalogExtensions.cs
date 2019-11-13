using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Rendering
{
  public static class PaperCatalogExtensions
  {
    public static void AddCatalog(this IPaperCatalog baseCatalog, IPaperCatalog paperCatalog)
    {
      var aggregateCatalog = baseCatalog as AggregateCatalog;
      if (aggregateCatalog == null)
        throw new Exception($"O catálogo base {baseCatalog.GetType().FullName} não suporta o mapeamento de um subcatálogo.");

      aggregateCatalog.AddCatalog(paperCatalog);
    }
  }
}
