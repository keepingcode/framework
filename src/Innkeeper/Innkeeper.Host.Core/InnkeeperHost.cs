using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Toolset;

namespace Innkeeper.Host.Core
{
  public static class InnkeeperHost
  {
    public static void Start(int port, string prefix, params string[] args)
    {
      try
      {
        var builder = WebHost
          .CreateDefaultBuilder(args)
          .UseInnkeeper(opts =>
          {
            opts.Prefixes.Add(prefix);
          })
          .UseStartup<Startup>();
        builder.Build().Run();
      }
      catch (Exception ex)
      {
        ex.Trace();
      }
    }
  }
}
