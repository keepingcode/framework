using Innkeeper.Host;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Toolset;

namespace Paper.Rendering
{
  class PaperPipeline : IPipeline
  {
    private readonly Regex Pattern = new Regex("^/Catalogs/([^/]+)/Papers/([^/]+)/?$");

    public async Task RunAsync(IRequestContext ctx, NextAsync next)
    {
      var path = ctx.Request.RequestPath;
      var match = Pattern.Match(path);
      if (match.Success)
      {
        var module = match.Groups[1].Value;
        var schema = match.Groups[2].Value;

        var writer = new StreamWriter(ctx.Response.Body);
        await writer.WriteAsync($"Rendering paper {schema} from module {module}...");
        await writer.FlushAsync();
      }
      else
      {
        await next();
      }
    }
  }
}
