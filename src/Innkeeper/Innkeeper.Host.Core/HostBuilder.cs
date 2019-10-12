using Innkeeper.Host;
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

namespace Innkeeper.Host.Core
{
  public static class HostBuilder
  {
    #region Configuring Host (IApplicationBuilder)

    public static IApplicationBuilder UseInnkeeper(this IApplicationBuilder app)
    {
      var objectFactory = app.ApplicationServices.GetRequiredService<IObjectFactory>();
      var pipelineTypes = ExposedTypes.GetTypes<IPipeline>();
      foreach (var pipelineType in pipelineTypes)
      {
        try
        {
          var pipeline = (IPipeline)objectFactory.CreateObject(pipelineType);
          app.Map(pipeline.Route, builder => builder.UseMiddleware<Middleware>(pipeline));
        }
        catch (Exception ex)
        {
          ex.Trace();
        }
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
          module.Configure(builder);
        }
        catch (Exception ex)
        {
          ex.Trace();
        }
      }
    }

    #endregion

    #region Configuring Pipeline (IWebHostBuilder)

    public static IWebHostBuilder UseInnkeeperHost(this IWebHostBuilder hostBuilder, Action<WebAppInfo> webAppBuilder)
    {
      var webAppInfo = new WebAppInfo();
      hostBuilder.ConfigureServices(services => services.AddSingleton<IWebAppInfo>(webAppInfo));

      webAppBuilder?.Invoke(webAppInfo);

      var port = webAppInfo.Port;

      hostBuilder.ConfigureAppConfiguration((ctx, config) => config.SetBasePath(App.Path));

      if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
      {
        var urlPrefix = CreateUrlPrefix(webAppInfo, appendPath: true);
        hostBuilder.UseHttpSys(httpSysOptions =>
        {
          httpSysOptions.AllowSynchronousIO = true;
          httpSysOptions.Authentication.Schemes = Microsoft.AspNetCore.Server.HttpSys.AuthenticationSchemes.Basic;
          httpSysOptions.Authentication.AllowAnonymous = true;
          httpSysOptions.MaxConnections = null;
          httpSysOptions.MaxRequestBodySize = 30000000;
          httpSysOptions.UrlPrefixes.Add(urlPrefix);
        });
      }
      else
      {
        var urlPrefix = CreateUrlPrefix(webAppInfo, appendPath: false);
        hostBuilder.UseUrls(urlPrefix);
      }

      return hostBuilder;
    }

    private static string CreateUrlPrefix(IWebAppInfo webAppInfo, bool appendPath)
    {
      var prefix = webAppInfo.PathPrefix;
      var port = webAppInfo.Port;

      if (!prefix.StartsWith("/")) prefix = $"/{prefix}";
      if (!prefix.EndsWith("/")) prefix += "/";

      return appendPath
        ? $"http://*:{port}{prefix}"
        : $"http://*:{port}/";
    }

    #endregion
  }
}
