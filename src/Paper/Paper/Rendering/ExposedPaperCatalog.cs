using Innkeeper.Host;
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
  class ExposedPaperCatalog : IPaperCatalog
  {
    private readonly HashMap<IPaperDescriptor> descriptorMap = new HashMap<IPaperDescriptor>();

    public ExposedPaperCatalog(IWebApp webApp)
    {
      Initialize(webApp);
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

    public ICollection<IPaperDescriptor> FindPaperDescriptor(string module)
    {
      var prefix = $"{module}/";
      var descriptors = (
        from entry in descriptorMap
        where entry.Key.StartsWith(prefix)
        select entry.Value
      ).ToArray();
      return descriptors;
    }

    public IPaperDescriptor FindPaperDescriptor(string module, string schema)
    {
      var id = $"{module}/{schema}";
      var descriptor = descriptorMap[id];
      return descriptor;
    }

    private void Initialize(IWebApp webApp)
    {
      Console.WriteLine("---PAPERS---");
      try
      {
        var factory = new PaperDescriptorFactory(webApp);
        var types = ExposedTypes.GetTypes<IPaper>();
        foreach (var type in types)
        {
          try
          {
            var descriptor = factory.CreatePaperDescriptor(type);
            var id = $"{descriptor.Catalog}/{descriptor.Paper}";
            descriptorMap[id] = descriptor;

            Console.WriteLine($"/Paper/Api/Catalogs/{descriptor.Catalog}/Papers/{descriptor.Paper}");
          }
          catch (Exception ex)
          {
            ex.Trace();
          }
        }
      }
      catch (Exception ex)
      {
        ex.Trace();
      }
      finally
      {
        Console.WriteLine("------");
      }
    }
  }
}