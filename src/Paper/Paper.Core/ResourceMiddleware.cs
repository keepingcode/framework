using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Net.Http.Headers;
using Paper.Media;
using Paper.Media.Design;
using Paper.Media.Serialization;
using Toolset;
using Microsoft.Extensions.DependencyInjection;
using Paper.Rendering;
using Paper.Browser.Web;

namespace Paper.Core
{
  public class ResourceMiddleware
  {
    private readonly RequestDelegate next;

    public ResourceMiddleware(RequestDelegate next)
    {
      this.next = next;
    }

    public async Task Invoke(HttpContext ctx, IServiceProvider serviceProvider)
    {
      try
      {
        var loader = new ResourceLoader();

        var resourcePath = ctx.Request.Path;
        var found = loader.FindResources(resourcePath);
        if (found.Ok)
        {
          using (var memory = new MemoryStream())
          {
            var ret = loader.LoadResource(resourcePath, memory);
            if (ret.Ok)
            {
              var buffer = memory.ToArray();
              await ctx.Response.Body.WriteAsync(buffer, 0, buffer.Length);
              return;
            }
          }
        }

        await next.Invoke(ctx);
      }
      catch (Exception ex)
      {
        ex.Trace();
        ctx.Response.StatusCode = 500;
      }
    }
  }
}