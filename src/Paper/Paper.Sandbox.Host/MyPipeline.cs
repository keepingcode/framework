using Innkeeper.Host;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Toolset;

namespace Paper.Sandbox.Host
{
  [Expose, Route("/My/Site")]
  class MyPipeline : IPipeline
  {
    public async Task RunAsync(IRequestContext ctx, NextAsync next)
    {
      var writer = new StreamWriter(ctx.Response.Body);
      await writer.WriteAsync($"Olá, mundo!");
      await writer.FlushAsync();
    }
  }
}
