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
    public Request(HttpContext context)
    {
      this.RequestUri = context.Request.GetDisplayUrl();
      this.PathBase = context.Request.PathBase;
      this.Path = context.Request.Path;
      this.Method = context.Request.Method;
      this.Headers = new Headers(context.Request.Headers);
      this.Body = context.Request.Body;
    }

    public string RequestUri { get; }

    public string PathBase { get; }

    public string Path { get; }

    public string Method { get; }

    public IHeaders Headers { get; }

    public Stream Body { get; }
  }
}
