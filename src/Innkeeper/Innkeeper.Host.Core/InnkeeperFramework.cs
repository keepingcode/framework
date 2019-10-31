using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolset;
using Toolset.Collections;
using Toolset.Reflection;

namespace Innkeeper.Host.Core
{
  public static class InnkeeperFramework
  {
    public static IServiceCollection AddPaper(this IServiceCollection services)
    {
      var builder = new ObjectFactoryBuilder(services);
      var objectFactory = builder.BuildObjectFactory();
      var modules = objectFactory.GetInstance<IModuleCollection>();
      modules.ForEach(module =>
      {
        try
        {
          module.Install(builder);
        }
        catch (Exception ex)
        {
          ex.Trace($"Falha invocando o método `Install' do módulo: {module?.GetType().FullName}");
        }
      });

      return services;
    }

    public static IApplicationBuilder UsePaper(this IApplicationBuilder app)
    {
      var objectFactory = app.ApplicationServices.GetRequiredService<IObjectFactory>();
      var router = objectFactory.GetInstance<IRouter>();

      app.UseMiddleware<PipelineMiddleware>();

      //var keys = router.Keys.OrderBy(x => x);

      //Console.WriteLine($"--routes--");

      //foreach (var key in keys)
      //{
      //  try
      //  {
      //    var route = router[key];
      //    var path = key.EndsWith("/") ? key.Substring(0, key.Length - 1) : key;
      //    app.Map(path, builder => builder.UseMiddleware<PipelineMiddleware>(route));

      //    Console.WriteLine(path);
      //  }
      //  catch (Exception ex)
      //  {
      //    ex.Trace();
      //  }
      //}

      //Console.WriteLine($"----");

      return app;
    }
  }
}
