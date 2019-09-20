using System;
using System.Collections.Generic;
using System.Text;

namespace Innkeeper.Pipelines
{
  public interface IRequestContext
  {
    IRequest Request { get; }

    IResponse Response { get; }
  }
}
