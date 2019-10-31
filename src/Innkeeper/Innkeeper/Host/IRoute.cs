using System;
using System.Collections.Generic;
using System.Text;

namespace Innkeeper.Host
{
  public interface IRoute
  {
    PathPrefix Path { get; }

    IPipeline CreatePipeline(IObjectFactory objectFactory);
  }
}
