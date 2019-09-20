using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Toolset.Collections;
using Toolset;
using Toolset.Net;

namespace Innkeeper.Pipelines
{
  public class Response : IResponse
  {
    private Request req;
    private IResponse res;

    public Response(Request request, IResponse response)
    {
      this.req = request;
      this.res = response;
    }

    public IHeaders Headers => res.Headers;

    public HttpStatusCode Status
    {
      get => res.Status;
      set => res.Status = value;
    }

    public Stream Body => res.Body;
  }
}
