using Innkeeper.Host;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Toolset;

namespace Innkeeper.Sandbox.Host
{
  [Expose]
  class MyPipeline : IPipeline
  {
    private readonly MyDependency myDependency;
    private readonly IHostInfo hostInfo;

    public void Configure(IRouter router)
    {
    }

    public MyPipeline(MyDependency myDependency, IHostInfo hostInfo)
    {
      this.myDependency = myDependency;
      this.hostInfo = hostInfo;
    }

    public async Task RunAsync(IRequestContext ctx, NextAsync next)
    {
      var message = $"{myDependency.Message} (App: {hostInfo.Name}_v{hostInfo.Version} / ID:{hostInfo.Guid.ToString("D").ToUpper()})";
      var buffer = UTF8Encoding.UTF8.GetBytes(message);
      await ctx.Response.Body.WriteAsync(buffer, 0, buffer.Length);
    }
  }
}
