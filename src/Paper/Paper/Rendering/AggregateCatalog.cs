using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolset.Collections;

namespace Paper.Rendering
{
  public class AggregateCatalog : IPaperCatalog
  {
    private Queue<IPaperCatalog> catalogQueue = new Queue<IPaperCatalog>();

    public AggregateCatalog()
    {
    }

    public AggregateCatalog(IEnumerable<IPaperCatalog> catalogs)
    {
      catalogs.ForEach(x => catalogQueue.Enqueue(x));
    }

    public void AddCatalog(params IPaperCatalog[] catalogs)
    {
      catalogs.ForEach(x => catalogQueue.Enqueue(x));
    }

    public ICollection<IPaperDescriptor> FindPaperDescriptor(Type paperType)
    {
      var descriptors = (
        from catalog in catalogQueue
        let descriptor = catalog.FindPaperDescriptor(paperType)
        select descriptor
      ).SelectMany().ToArray();
      return descriptors;
    }

    public ICollection<IPaperDescriptor> FindPaperDescriptor(string module)
    {
      var descriptors = (
        from catalog in catalogQueue
        let descriptor = catalog.FindPaperDescriptor(module)
        select descriptor
      ).SelectMany().ToArray();
      return descriptors;
    }

    public IPaperDescriptor FindPaperDescriptor(string module, string schema)
    {
      var result = (
        from catalog in catalogQueue
        let descriptor = catalog.FindPaperDescriptor(module, schema)
        where descriptor != null
        select descriptor
      ).FirstOrDefault();
      return result;
    }
  }
}
