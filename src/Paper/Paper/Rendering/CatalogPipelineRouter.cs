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
  class CatalogPipelineRouter : IPipelineRouter
  {
    private IWebApp webApp;

    public CatalogPipelineRouter(IWebApp webApp)
    {
      this.webApp = webApp;
    }

    public void Map(IRouter map)
    {
      var path = CreatePath(webApp.Name);
      map.Map<CatalogPipeline>(path);
    }

    public static string CreatePath(string catalog)
    {
      return $"/Paper/Api/Catalogs/{catalog}";
    }

    public static string CreatePath(string catalog, string paper)
    {
      return $"/Paper/Api/Catalogs/{catalog}/Papers/{paper}";
    }

    public static Schema ExplainPath(string path)
    {
      var schema = new Schema();
      var regex = new Regex(@"^/Paper/Api/Catalogs/([^/?]+)(?:/Papers/([^/?]+)(?:/Actions/([^/?]+))?)?");
      var match = regex.Match(path);
      if (match.Success)
      {
        var catalog = match.Groups[1].Value;
        var paper = match.Groups[2].Value;
        var action = match.Groups[3].Value;

        schema.Catalog = string.IsNullOrEmpty(catalog) ? null : catalog;
        schema.Paper = string.IsNullOrEmpty(paper) ? null : paper;
        schema.Action = string.IsNullOrEmpty(action) ? null : action;
      }
      return schema;
    }

    public class Schema
    {
      public string Catalog { get; set; }
      public string Paper { get; set; }
      public string Action { get; internal set; }

      public override string ToString()
      {
        var text = "";
        if (Catalog != null) text += $"Catálogo:{Catalog}";
        if (Paper != null) text += $"/Paper:{Paper}";
        if (Action != null) text += $"/Ação:{Action}";
        return text;
      }
    }
  }
}