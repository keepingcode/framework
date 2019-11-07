using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Toolset;
using Toolset.Xml;
using Microsoft.Extensions.DependencyInjection;
using Innkeeper.Host;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.IO.Compression;
using Toolset.Net;

namespace Innkeeper.Host.Core
{
  internal class PipelineMiddleware
  {
    private readonly RequestDelegate next;

    public PipelineMiddleware(RequestDelegate next)
    {
      this.next = next;
    }

    public async Task Invoke(HttpContext httpContext, IServiceProvider serviceProvider)
    {
      try
      {
        var objectFactory = serviceProvider.GetService<IObjectFactory>();
        if (objectFactory == null)
          throw new NullReferenceException("A instância de IObjectFactory não foi definida no IServiceProvider.");

        var ctx = new RequestContext();
        ctx.Request = new Request(ctx, httpContext);
        ctx.Response = new Response(ctx, httpContext);

        var pipelineInvoker = new PipelineInvoker();

        await pipelineInvoker.InvokeAsync(ctx, objectFactory, () => next.Invoke(httpContext));
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