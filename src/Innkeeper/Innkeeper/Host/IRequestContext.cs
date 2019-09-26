using System;
using System.Collections.Generic;
using System.Text;

namespace Innkeeper.Host
{
  public interface IRequestContext
  {
    IRequest Request { get; }

    IResponse Response { get; }
  }
}
