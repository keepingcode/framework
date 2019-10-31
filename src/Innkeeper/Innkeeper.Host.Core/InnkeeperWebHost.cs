using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Toolset;
using Toolset.Collections;

namespace Innkeeper.Host.Core
{
  public static class InnkeeperWebHost
  {
    private class Parts
    {
      public WebApp WebApp { get; set; }

      public Router Router { get; set; }

      public ModuleCollection ModuleCollection { get; set; }
    }

    public static void Run(params string[] args)
    {
      Run(options: null, args: args);
    }

    public static void Run(Action<WebApp> options, params string[] args)
    {
      CreateDefaultBuilder(options, args)
        .UseStartup<DefaultStartup>()
        .Build()
        .Run();
    }

    public static IWebHostBuilder CreateDefaultBuilder(params string[] args)
    {
      return CreateDefaultBuilder(options: null, args: args);
    }

    public static IWebHostBuilder CreateDefaultBuilder(Action<WebApp> options, params string[] args)
    {
      var builder = new WebHostBuilder();
      var webApp = IdentifyWebApp(options);
      var router = Router.Create(webApp);
      var moduleCollection = ModuleCollection.Create(webApp);
      var context = new Parts
      {
        WebApp = webApp,
        Router = router,
        ModuleCollection = moduleCollection
      };

      BuildInnkeeperHost(builder, context, args);
      BuildInnkeeperPlatform(builder, context, args);
      return builder;
    }

    public static IWebHostBuilder UseInnkeeperPlatform(this IWebHostBuilder webHostBuilder, params string[] args)
    {
      return UseInnkeeperPlatform(webHostBuilder, options: null, args: args);
    }

    public static IWebHostBuilder UseInnkeeperPlatform(
      this IWebHostBuilder webHostBuilder, Action<WebApp> options, params string[] args)
    {
      var webApp = IdentifyWebApp(options);
      var router = Router.Create(webApp);
      var moduleCollection = ModuleCollection.Create(webApp);
      var context = new Parts
      {
        WebApp = webApp,
        Router = router,
        ModuleCollection = moduleCollection
      };

      BuildInnkeeperPlatform(webHostBuilder, context, args);
      return webHostBuilder;
    }

    private static void BuildInnkeeperHost(IWebHostBuilder builder, Parts parts, string[] args)
    {
      var webApp = parts.WebApp;
      var router = parts.Router;

      builder
        .ConfigureAppConfiguration((ctx, config) =>
        {
          var env = ctx.HostingEnvironment;

          config.SetBasePath(App.Path);
          config
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

          if (env.IsDevelopment())
          {
            var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
            if (appAssembly != null)
            {
              config.AddUserSecrets(appAssembly, optional: true);
            }
          }

          config.AddEnvironmentVariables();

          if (args != null)
          {
            config.AddCommandLine(args);
          }
        })
        .UseContentRoot(Directory.GetCurrentDirectory())
        .ConfigureLogging((hostingContext, logging) =>
        {
          //logging.UseConfiguration(hostingContext.Configuration.GetSection("Logging"));
          logging.AddConsole();
          logging.AddDebug();
        })
        .UseDefaultServiceProvider((context, options) =>
        {
          options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
        });

      if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
      {
        builder.UseHttpSys(httpSysOptions =>
        {
          httpSysOptions.AllowSynchronousIO = true;
          httpSysOptions.Authentication.Schemes = Microsoft.AspNetCore.Server.HttpSys.AuthenticationSchemes.Basic;
          httpSysOptions.Authentication.AllowAnonymous = true;
          httpSysOptions.MaxConnections = null;
          httpSysOptions.MaxRequestBodySize = 30000000;

          if (router.Keys.Any())
          {
            foreach (var key in router.Keys)
            {
              var prefix = $"http://*:{webApp.Port}{key}";
              httpSysOptions.UrlPrefixes.Add(prefix);
            }
          }
          else
          {
            var prefix = $"http://*:{webApp.Port}{webApp.UrlPrefix}";
            httpSysOptions.UrlPrefixes.Add(prefix);
          }

        });
      }
      else
      {
        var prefix = $"http://*:{webApp.Port}/";
        builder
          .UseKestrel()
          .ConfigureServices(services =>
            services.AddTransient<IConfigureOptions<KestrelServerOptions>, KestrelServerOptionsSetup>()
          )
          .UseUrls(prefix);
      }
    }

    private static void BuildInnkeeperPlatform(IWebHostBuilder builder, Parts parts, string[] args)
    {
      builder.ConfigureServices(services =>
      {
        services
          .AddSingleton<IWebApp>(parts.WebApp)
          .AddSingleton<IRouter>(parts.Router)
          .AddSingleton<IModuleCollection>(parts.ModuleCollection);
      });
    }

    private static WebApp IdentifyWebApp(Action<WebApp> options)
    {
      var webApp = new WebApp();

      // Parametros iniciais
      //
      webApp.Guid = App.Guid;
      webApp.Port = WebApp.DefaultPort;
      webApp.Name = App.Name;
      webApp.Description = App.Description;
      webApp.Version = App.Version;

      // Personalizacao dos parametros
      //
      options?.Invoke(webApp);

      // Revisao dos parametros finais
      //
      if (webApp.Port == 0)
      {
        webApp.Port = WebApp.DefaultPort;
      }
      if (string.IsNullOrEmpty(webApp.UrlPrefix))
      {
        webApp.UrlPrefix = "/";
      }

      // Sanitizando o prefixo de Url
      //
      var tokens = webApp.UrlPrefix.Split('/', '\\').NotNullOrEmpty();
      webApp.UrlPrefix = "/" + string.Join("/", tokens);

      return webApp;
    }
  }
}
