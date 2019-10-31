using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolset;
using Toolset.Collections;

namespace Innkeeper.Host.Core
{
  public class WebApp : IWebApp
  {
    public const int DefaultPort = 90;

    internal WebApp()
    {
    }

    public Guid Guid { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public VersionInfo Version { get; set; }

    public int Port { get; set; }

    public string UrlPrefix { get; set; }

    internal static string MakePath(string hostName)
    {
      if (string.IsNullOrEmpty(hostName)) return null;

      var tokens = hostName.Split('.').NotNullOrEmpty();
      var path = "/" + string.Join("/", tokens);
      return path;
    }
  }
}
