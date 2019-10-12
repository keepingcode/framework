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
    private readonly MyDependency myDependency;
    private readonly IWebAppInfo webAppInfo;

    public string Route { get; }

    public MyPipeline(MyDependency myDependency, IWebAppInfo webAppInfo)
    {
      this.myDependency = myDependency;
      this.webAppInfo = webAppInfo;
    }

    public async Task RenderAsync(IRequestContext ctx, NextAsync next)
    {
      var message = $"{myDependency.Message} (App: {webAppInfo.Name}_v{webAppInfo.Version} / ID:{webAppInfo.Guid.ToString("D").ToUpper()})";
      var buffer = UTF8Encoding.UTF8.GetBytes(message);
      await ctx.Response.Body.WriteAsync(buffer, 0, buffer.Length);
    }
  }
}
