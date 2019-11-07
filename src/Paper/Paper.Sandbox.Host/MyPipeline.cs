using Innkeeper.Host;
using Innkeeper.Rest;
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
  class MyPipeline : RestPipeline
  {
    [Get("/")]
    public async Task Echo()
    {
      await Res.SendAsync($"Olá, mundo!");
    }
  }
}
