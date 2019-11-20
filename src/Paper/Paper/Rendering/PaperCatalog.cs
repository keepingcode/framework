using Paper.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolset;
using Toolset.Collections;
using Toolset.Reflection;

namespace Paper.Rendering
{
  class PaperCatalog : IPaperCatalog
  {
    private HashMap<IPaperDescriptor> descriptorMap = new HashMap<IPaperDescriptor>();

    public void MapPaperDescriptor(IPaperDescriptor descriptor)
    {
      var id = $"{descriptor.Catalog}/{descriptor.Paper}";
      descriptorMap[id] = descriptor;
    }

    public IPaperDescriptor MapPaperDescriptor(string catalog, string paper, Type paperType)
    {
      var descriptor = new PaperDescriptor
      {
        Catalog = catalog,
        Paper = paper,
        PaperType = paperType
      };
      var id = $"{descriptor.Catalog}/{descriptor.Paper}";
      descriptorMap[id] = descriptor;
      return descriptor;
    }

    public ICollection<IPaperDescriptor> FindPaperDescriptor(Type paperType)
    {
      var descriptors = (
        from entry in descriptorMap
        where entry.Value.PaperType == paperType
        select entry.Value
      ).ToArray();
      return descriptors;
    }

    public ICollection<IPaperDescriptor> FindPaperDescriptor(string catalog)
    {
      var prefix = $"{catalog}/";
      var descriptors = (
        from entry in descriptorMap
        where entry.Key.StartsWith(prefix)
        select entry.Value
      ).ToArray();
      return descriptors;
    }

    public IPaperDescriptor FindPaperDescriptor(string catalog, string paper)
    {
      var id = $"{catalog}/{paper}";
      var descriptor = descriptorMap[id];
      return descriptor;
    }
  }
}