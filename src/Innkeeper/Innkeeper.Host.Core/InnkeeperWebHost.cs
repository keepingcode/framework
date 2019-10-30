using System;
using System.IO;
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

namespace Innkeeper.Host.Core
{
  public static class InnkeeperWebHost
  {
    public static void Run(params string[] args)
    {
      Run(options: null, args: args);
    }

    public static void Run(Action<HostInfo> options, params string[] args)
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

    public static IWebHostBuilder CreateDefaultBuilder(Action<HostInfo> options, params string[] args)
    {
      var builder = new WebHostBuilder();
      var hostInfo = CollectHostInfo(options);
      BuildInnkeeperHost(builder, hostInfo, args);
      BuildInnkeeperPlatform(builder, hostInfo, args);
      return builder;
    }

    public static IWebHostBuilder UseInnkeeperPlatform(this IWebHostBuilder webHostBuilder, params string[] args)
    {
      return UseInnkeeperPlatform(webHostBuilder, options: null, args: args);
    }

    public static IWebHostBuilder UseInnkeeperPlatform(
      this IWebHostBuilder webHostBuilder, Action<HostInfo> options, params string[] args)
    {
      var hostInfo = CollectHostInfo(options);
      BuildInnkeeperPlatform(webHostBuilder, hostInfo, args);
      return webHostBuilder;
    }

    private static void BuildInnkeeperHost(IWebHostBuilder builder, HostInfo hostInfo, string[] args)
    {
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
        var prefix = $"http://*:{hostInfo.Port}{hostInfo.UrlPrefix}";
        builder.UseHttpSys(httpSysOptions =>
        {
          httpSysOptions.AllowSynchronousIO = true;
          httpSysOptions.Authentication.Schemes = Microsoft.AspNetCore.Server.HttpSys.AuthenticationSchemes.Basic;
          httpSysOptions.Authentication.AllowAnonymous = true;
          httpSysOptions.MaxConnections = null;
          httpSysOptions.MaxRequestBodySize = 30000000;
          httpSysOptions.UrlPrefixes.Add(prefix);
        });
      }
      else
      {
        var prefix = $"http://*:{hostInfo.Port}/";
        builder
          .UseKestrel()
          .ConfigureServices(services =>
            services.AddTransient<IConfigureOptions<KestrelServerOptions>, KestrelServerOptionsSetup>()
          )
          .UseUrls(prefix);
      }
    }

    private static void BuildInnkeeperPlatform(IWebHostBuilder builder, HostInfo hostInfo, string[] args)
    {
      builder.ConfigureServices(services =>
      {
        var objectFactoryBuilder = new ObjectFactoryBuilder(services);
        Modules.ConfigureServices(objectFactoryBuilder);
      });
    }

    private static HostInfo CollectHostInfo(Action<HostInfo> options)
    {
      var hostInfo = new HostInfo();

      // Parametros iniciais
      //
      hostInfo.Guid = App.Guid;
      hostInfo.Port = HostInfo.DefaultPort;
      hostInfo.Name = App.Name;
      hostInfo.Description = App.Description;
      hostInfo.Version = App.Version;

      // Personalizacao dos parametros
      //
      options?.Invoke(hostInfo);

      // Revisao dos parametros finais
      //
      if (hostInfo.Port == 0)
      {
        hostInfo.Port = HostInfo.DefaultPort;
      }
      if (string.IsNullOrEmpty(hostInfo.UrlPrefix))
      {
        hostInfo.UrlPrefix = HostInfo.MakePath(hostInfo.Name);
      }

      return hostInfo;
    }
  }
}
