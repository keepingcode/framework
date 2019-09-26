using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Innkeeper.Host.Core
{
  public class RequestContext : IRequestContext
  {
    public IRequest Request { get; internal set; }

    public IResponse Response { get; internal set; }
  }
}