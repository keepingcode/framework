using Innkeeper.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Toolset;

namespace Paper.Rendering
{
  [Expose]
  class PaperPipelineRouter : IPipelineRouter
  {
    private IWebApp webApp;

    public PaperPipelineRouter(IWebApp webApp)
    {
      this.webApp = webApp;
    }

    public void Map(IRouter map)
    {
      var path = CreatePath(webApp.Name);
      map.Map<PaperPipeline>(path);
    }

    public static string CreatePath(string module)
    {
      return $"/Paper/Api/1/Catalogs/{module}";
    }

    public static string CreatePath(string module, string schema)
    {
      return $"/Paper/Api/1/Catalogs/{module}/Papers/{schema}";
    }

    public static string ExtractTokensFromPath(string path)
    {
      if (!path.StartsWith("/Paper/Api/1/Catalogs/"))
        return null;

      var tokens = path.Split('?').First().Split('/');
      var module = tokens[5];
      return module;
    }

    public static void ExtractTokensFromPath(string path, out string module)
    {
      module = null;
      if (path.StartsWith("/Paper/Api/1/Catalogs/"))
      {
        var tokens = path.Split('?').First().Split('/');
        module = tokens[5];
      }
    }

    public static void ExtractTokensFromPath(string path, out string module, out string schema)
    {
      module = null;
      schema = null;

      var regex = new Regex(@"^/Paper/Api/1/Catalogs/([^/?]+)(?:/Papers/([^/?]+))?");
      var match = regex.Match(path);
      if (match.Success)
      {
        module = match.Groups[1].Value;
        schema = match.Groups[2].Value;
      }
    }
  }
}