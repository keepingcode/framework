//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using Toolset;

//namespace Innkeeper.Host.Core
//{
//  public static class AspNetCoreExtensions
//  {
//    public static IWebHostBuilder UseInnkeeper(this IWebHostBuilder webHostBuilder, Action<HostInfo> options)
//    {
//      var hostBuilder = new HostBuilder();
//      hostBuilder.BuildHost(webHostBuilder, options);
//      return webHostBuilder;
//    }

//    public static IWebHostBuilder UseInnkeeperHost(this IWebHostBuilder webHostBuilder, Action<HostInfo> options)
//    {
//      var hostBuilder = new HostBuilder();
//      hostBuilder.BuildHost(webHostBuilder, options);
//      return webHostBuilder;
//    }

//    public static IServiceCollection AddInnkeeper(this IServiceCollection services)
//    {
//      var builder = new ObjectFactoryBuilder(services);
//      builder.AddObjectFactory();

//      Modules.ConfigureServices(builder);

//      return services;
//    }

//    public static IApplicationBuilder UseInnkeeper(this IApplicationBuilder app)
//    {
//      var objectFactory = app.ApplicationServices.GetRequiredService<IObjectFactory>();
//      var pipelineTypes = ExposedTypes.GetTypes<IPipeline>();
//      foreach (var pipelineType in pipelineTypes)
//      {
//        Console.WriteLine($"--routes--");
//        try
//        {
//          // var router = new Router();
//          // var pipeline = (IPipeline)objectFactory.CreateObject(pipelineType, router);
//          // if (router.Any())
//          // {
//          //   foreach (var route in router)
//          //   {
//          //     app.Map(route, builder => builder.UseMiddleware<PipelineMiddleware>(pipeline));
//          //     Console.WriteLine($"{route} => {pipeline.GetType().FullName}");
//          //   }
//          // }
//          // else
//          // {
//          //   app.UseMiddleware<PipelineMiddleware>(pipeline);
//          //   Console.WriteLine($"/ => {pipeline.GetType().FullName}");
//          // }
//        }
//        catch (Exception ex)
//        {
//          ex.Trace();
//        }
//        finally
//        {
//          Console.WriteLine($"----");
//        }
//      }
//      return app;
//    }
//  }
//}
