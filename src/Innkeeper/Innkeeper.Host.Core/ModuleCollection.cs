using System;
using System.Collections.Generic;
using System.Text;
using Toolset;

namespace Innkeeper.Host.Core
{
  internal class ModuleCollection : List<IModule>, IModuleCollection
  {
    private ModuleCollection()
    {
    }

    public static ModuleCollection Create(WebApp webApp)
    {
      var modules = new ModuleCollection();
      var types = ExposedTypes.GetTypes<IModule>();
      foreach (var type in types)
      {
        try
        {
          var module = (IModule)Activator.CreateInstance(type);
          modules.Add(module);
        }
        catch (Exception ex)
        {
          ex.Trace();
        }
      }
      return modules;
    }
  }
}