using Innkeeper.Host;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Toolset;

namespace Sandbox.Host
{
  [Expose]
  class MyPipeline : IPipeline
  {
    private IObjectFactory factory;

    public string Route { get; }

    public MyPipeline(IObjectFactory factory)
    {
      this.factory = factory;
    }

    public async Task RenderAsync(IRequestContext ctx, NextAsync next)
    {
      var myDependency = this.factory.GetInstance<MyDependency>();
      var buffer = UTF8Encoding.UTF8.GetBytes(myDependency.Message);
      await ctx.Response.Body.WriteAsync(buffer, 0, buffer.Length);
    }
  }
}
