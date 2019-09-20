using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Innkeeper.Pipelines
{
  public interface IPipeline
  {
    string Route { get; }

    Task RenderAsync(IRequestContext ctx, NextAsync next);
  }
}
