using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Innkeeper.Host;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Toolset;
using Toolset.Collections;

namespace Innkeeper.Host.Core
{
  internal class Request : IRequest
  {
    private readonly Stream baseBody;
    private Stream _body;

    public Request(IRequestContext requestContext, HttpContext context)
    {
      this.baseBody = context.Request.Body;
      this.RequestContext = requestContext;
      this.RequestUri = context.Request.GetDisplayUrl();
      this.RequestPath = context.Request.PathBase + context.Request.Path;
      this.PathBase = context.Request.PathBase;
      this.Path = context.Request.Path;
      this.Method = context.Request.Method;
      this.Headers = new Headers(context.Request.Headers);
      this.QueryArgs = new QueryArgs(context.Request.QueryString.Value);
    }

    public IRequestContext RequestContext { get; }

    public string RequestUri { get; }

    public string RequestPath { get; set; }

    public string PathBase { get; }

    public string Path { get; }

    public string Method { get; }

    public IHeaders Headers { get; }

    public IArgs QueryArgs { get; }

    public Stream Body
    {
      get => _body ?? baseBody;
      private set => _body = value;
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
