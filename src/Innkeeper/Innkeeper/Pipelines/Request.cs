using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Innkeeper.Pipelines
{
  public class Request : IRequest
  {
    private IRequest request;

    public Request(IRequest request)
    {
      this.request = request;
      this.QueryArgs = Args.ParseQueryArgs(request.RequestUri);
    }

    public string RequestUri => request.RequestUri;

    public string PathBase => request.PathBase;

    public string Path => request.Path;

    public string Method => request.Method;

    public IHeaders Headers => request.Headers;

    public Stream Body => request.Body;

    public IArgs QueryArgs { get; }
  }
}