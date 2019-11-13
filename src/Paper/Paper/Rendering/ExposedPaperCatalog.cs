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
    private HashMap<IPaperDescriptor> descriptorMap = new HashMap<IPaperDescriptor>();

    public ExposedPaperCatalog()
    {
      Initialize();
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

    private void Initialize()
    {
      Console.WriteLine("---PAPERS---");
      try
      {
        var types = ExposedTypes.GetTypes<IPaper>();
        foreach (var type in types)
        {
          try
          {
            var descriptor = new PaperDescriptor
            {
              Module = PaperDescriptor.IdentifyModule(type),
              Schema = PaperDescriptor.IdentifySchema(type),
              PaperType = type
            };
            var id = $"{descriptor.Module}/{descriptor.Schema}";
            descriptorMap[id] = descriptor;

            Console.WriteLine($"{PaperPipelineRouter.CreatePath(descriptor.Module, descriptor.Schema)}");
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