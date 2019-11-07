using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Innkeeper.Host;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Toolset.Collections;

namespace Innkeeper.Host.Core
{
  internal class Response : IResponse
  {
    private readonly Microsoft.AspNetCore.Http.HttpResponse res;

    private readonly Stream baseBody;
    private Stream _body;

    public Response(IRequestContext requestContext, HttpContext context)
    {
      this.res = context.Response;
      this.baseBody = context.Response.Body;
      this.RequestContext = requestContext;
      this.Headers = new Headers(context.Response.Headers);
    }

    public IRequestContext RequestContext { get; }

    public IHeaders Headers { get; }

    public Stream Body
    {
      get => _body ?? baseBody;
      private set => _body = value;
    }

    public HttpStatusCode Status
    {
      get => (HttpStatusCode)res.StatusCode;
      set => res.StatusCode = (int)value;
    }

    public IRequestContext GetContext()
    {
      return RequestContext;
    }

    public void SetBody(Func<Stream, Stream> bodyMaker)
    {
      Body = bodyMaker.Invoke(baseBody);
    }
  }
}