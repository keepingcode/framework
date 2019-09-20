using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Innkeeper.Pipelines;
using Microsoft.AspNetCore.Http;
using Toolset.Collections;

namespace Innkeeper.Host
{
  internal class Response : IResponse
  {
    private readonly Microsoft.AspNetCore.Http.HttpResponse res;

    public Response(HttpContext context)
    {
      this.res = context.Response;
      this.Headers = new Headers(context.Response.Headers);
    }

    public IHeaders Headers { get; }

    public Stream Body
    {
      get => res.Body;
    }

    public HttpStatusCode Status
    {
      get => (HttpStatusCode)res.StatusCode;
      set => res.StatusCode = (int)value;
    }
  }
}