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
using Toolset.Reflection;

namespace Innkeeper.Host.Core
{
  public class HostBuilder
  {
    public void BuildHost(IWebHostBuilder webHostBuilder, Action<HostInfo> options)
    {
      // Configuração básica
      //
      webHostBuilder.ConfigureAppConfiguration((ctx, config) => config.SetBasePath(App.Path));

      // Escolhendo e levantando o barramento
      //

      var hostInfo = CollectHostInfo(options);

      
      /*
      if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
      {
        var urlPrefix = "";// CreateUrlPrefix(hostInfo, appendPath: true);
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
        var urlPrefix = "";/CreateUrlPrefix(hostInfo, appendPath: false);
        hostBuilder.UseUrls(urlPrefix);
      }
      */
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
