using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Innkeeper.Host
{
  public interface IPipeline
  {
    Task RenderAsync(IRequestContext ctx, NextAsync next);
  }
}
