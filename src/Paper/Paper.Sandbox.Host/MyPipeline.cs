using Innkeeper.Host;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Toolset;

namespace Paper.Sandbox.Host
{
  [Expose]
  public class MyPipeline : IPipeline
  {
    private readonly IWebHost webAppInfo;
    private readonly SomeDependency myDependency;

    public MyPipeline(IRouter router, IWebHost webAppInfo, SomeDependency myDependency)
    {
      router.Map("/My/Paper");
      this.webAppInfo = webAppInfo;
      this.myDependency = myDependency;
    }

    public async Task RenderAsync(IRequestContext ctx, NextAsync next)
    {
      var message = $"{myDependency.Message} (App: {webAppInfo.Name}_v{webAppInfo.Version} / ID:{webAppInfo.Guid.ToString("D").ToUpper()})";
      var buffer = UTF8Encoding.UTF8.GetBytes(message);
      await ctx.Response.Body.WriteAsync(buffer, 0, buffer.Length);
    }
  }
}
