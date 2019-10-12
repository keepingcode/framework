using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolset;
using Toolset.Collections;

namespace Innkeeper.Host.Core
{
  public class WebAppInfo : IWebAppInfo
  {
    private string _pathPrefix;

    internal WebAppInfo()
    {
    }

    public Guid Guid
    {
      get => App.Guid;
      set => App.Guid = value;
    }

    public string Name
    {
      get => App.Name;
      set => App.Name = value;
    }

    public string Description
    {
      get => App.Description;
      set => App.Description = value;
    }

    public VersionInfo Version
    {
      get => App.Version;
      set => App.Version = value;
    }

    public int Port { get; set; } = 90;

    public string PathPrefix
    {
      get => _pathPrefix ?? MakePathFromName(Name);
      set => _pathPrefix = value;
    }

    internal static string MakePathFromName(string name)
    {
      if (name == null)
        return null;

      var tokens = name.Split('.').NonNullOrEmpty().ToArray();
      var path = SanitizePath(tokens);
      return path;
    }
    private static string SanitizePath(params string[] parts)
    {
      var tokens = parts.SelectMany(x => x.Split('/', '\\').NonNullOrEmpty());
      var path = $"/{string.Join("/", tokens)}";
      if (!path.EndsWith("/"))
      {
        path += "/";
      }
      return path;
    }
  }
}
