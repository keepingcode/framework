using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;
using Toolset;
using Toolset.Xml;
using Microsoft.Extensions.DependencyInjection;
using Innkeeper.Pipelines;
using Microsoft.AspNetCore.Http;

namespace Innkeeper.Host
{
  internal class Middleware
  {
    private readonly IPipeline pipeline;
    private readonly RequestDelegate next;

    public Middleware(IPipeline pipeline, RequestDelegate next)
    {
      this.pipeline = pipeline;
      this.next = next;
    }

    public async Task Invoke(HttpContext httpContext, IServiceProvider serviceProvider)
    {
      try
      {
        var req = new Pipelines.Request(new Request(httpContext));
        var res = new Pipelines.Response(req, new Response(httpContext));
        var ctx = new RequestContext { Request = req, Response = res };
        
        var factory = serviceProvider.GetService<IObjectFactory>();
        if (factory == null)
          throw new NullReferenceException("A instância de IObjectFactory não foi definida no IServiceProvider.");

        var nextAsync = new NextAsync(async () => await next(httpContext));
        await pipeline.RenderAsync(ctx, nextAsync);
      }
      catch (Exception ex)
      {
        ex.Trace();

        var req = httpContext.Request;
        var res = httpContext.Response;
        var status = HttpStatusCode.InternalServerError;

        res.StatusCode = (int)status;
        res.ContentType = "text/plain; charset=UTF-8";

        var ln = Environment.NewLine;
        await res.WriteAsync(
          $"{(int)status} - {status.ToString().ChangeCase(TextCase.ProperCase)}{ln}{ex.Message}{ln}Caused by:{ln}{ex.GetStackTrace()}"
        );
      }
    }
  }
}