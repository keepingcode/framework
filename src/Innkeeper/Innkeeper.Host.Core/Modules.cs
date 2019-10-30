using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolset;
using Toolset.Reflection;

namespace Innkeeper.Host.Core
{
  public static class Modules
  {
    private static readonly IInnkeeperModule[] modules;

    static Modules()
    {
      modules = LoadModules().ToArray();
    }

    private static IEnumerable<IInnkeeperModule> LoadModules()
    {
      var types = ExposedTypes.GetTypes<IInnkeeperModule>();
      foreach (var type in types)
      {
        IInnkeeperModule module = null;
        try
        {
          module = (IInnkeeperModule)Activator.CreateInstance(type);
        }
        catch (Exception ex)
        {
          ex.Trace();
        }
        if (module != null)
        {
          yield return module;
        }
      }
    }

    public static void ConfigureServices(IObjectFactoryBuilder builder)
    {
      foreach (var module in modules)
      {
        try
        {
          if (module._Has("ConfigureServices"))
          {
            // FIXME: A invocação do método deveria ser dinâmica, permitindo que o
            // módulo definisse seus próprios parâmetros, dentre aqueles disponíveis
            // neste momento de ignição do módulo.
            // Algo como:
            //     objectFactory.Call(module, "ConfigureServices");
            module._Call("ConfigureServices", builder);
          }
        }
        catch (Exception ex)
        {
          ex.Trace();
        }
      }
    }

    public static void Configure(Microsoft.AspNetCore.Builder.IApplicationBuilder app)
    {
      foreach (var module in modules)
      {
        try
        {
          if (module._Has("Configure"))
          {
            // FIXME: A invocação do método deveria ser dinâmica, permitindo que o
            // módulo definisse seus próprios parâmetros, dentre aqueles disponíveis
            // neste momento de ignição do módulo.
            // Algo como:
            //     objectFactory.Call(module, "Configure");
            // TODO: Ainda não suportado
            // module._Call("Configure", ...);
          }
        }
        catch (Exception ex)
        {
          ex.Trace();
        }
      }
    }
  }
}
