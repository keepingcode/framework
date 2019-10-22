﻿using Innkeeper.Host.Core;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Toolset;

namespace Sandbox.Host
{
  public static class Program
  {
    public static void Main(string[] args)
    {
      try
      {
        var builder = WebHost
          .CreateDefaultBuilder(args)
          .UseInnkeeperHost(app => {
            app.Port = 9090;
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