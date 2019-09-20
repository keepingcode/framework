using Innkeeper.Modules;
using Innkeeper.Pipelines;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using Toolset;
using Toolset.Collections;

namespace Innkeeper.Host
{
  public static class Extensions
  {
    #region Configuring Host (IApplicationBuilder)

    public static IApplicationBuilder UseInnkeeper(this IApplicationBuilder app)
    {
      var objectFactory = app.ApplicationServices.GetRequiredService<IObjectFactory>();
      var pipelineTypes = ExposedTypes.GetTypes<IPipeline>();
      foreach (var pipelineType in pipelineTypes)
      {
        var pipeline = (IPipeline)objectFactory.CreateObject(pipelineType);
        app.Map(pipeline.Route, builder => builder.UseMiddleware<Middleware>(pipeline));
      }
      return app;
    }

    #endregion

    #region Configuring Services (AddInnkeeper)

    public static IServiceCollection AddInnkeeper(this IServiceCollection services)
    {
      var builder = new ObjectFactoryBuilder(services);
      builder.AddObjectFactory();

      IgniteModules(builder);

      return services;
    }

    private static void IgniteModules(IObjectFactoryBuilder builder)
    {
      var types = ExposedTypes.GetTypes<IModule>();
      foreach (var type in types)
      {
        try
        {
          var module = (IModule)Activator.CreateInstance(type);
          module.Ignite(builder);
        }
        catch (Exception ex)
        {
          ex.Trace();
        }
      }
    }

    #endregion

    #region Configuring Pipeline (IWebHostBuilder)

    public static IWebHostBuilder UseInnkeeper(this IWebHostBuilder builder, string prefix)
    {
      return UseInnkeeper(builder, opts => opts.AddPrefix(prefix));
    }

    public static IWebHostBuilder UseInnkeeper(this IWebHostBuilder builder, Action<Options> opts)
    {
      var options = new Options();

      opts.Invoke(options);

      var urlPrefixes = options.Prefixes.Select(CreateUrlPrefix).ToArray();

      builder.ConfigureAppConfiguration((ctx, config) => config.SetBasePath(App.Path));

      if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
      {
        builder.UseHttpSys(httpSysOptions =>
        {
          httpSysOptions.AllowSynchronousIO = true;
          httpSysOptions.Authentication.Schemes = Microsoft.AspNetCore.Server.HttpSys.AuthenticationSchemes.Basic;
          httpSysOptions.Authentication.AllowAnonymous = true;
          httpSysOptions.MaxConnections = null;
          httpSysOptions.MaxRequestBodySize = 30000000;
          httpSysOptions.MaxAccepts = 30000000;
          urlPrefixes.ForEach(urlPrefix => httpSysOptions.UrlPrefixes.Add(urlPrefix));
        });
      }
      else
      {
        builder.UseUrls(urlPrefixes);
      }

      builder.UseStartup<Startup>();

      return builder;
    }

    private static string CreateUrlPrefix(string prefix)
    {
      if (!prefix.StartsWith("/")) prefix = $"/{prefix}";
      if (!prefix.EndsWith("/")) prefix += "/";

      return $"http://localhost:90{prefix}";
    }

    #endregion
  }
}
