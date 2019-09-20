using Innkeeper.Host;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Toolset;

namespace Sandbox.Host
{
  static class Program
  {
    static void Main(string[] args)
    {
      try
      {
        var builder = WebHost
          .CreateDefaultBuilder(args)
          .UseInnkeeper(opts => opts.AddPrefix("/").AddPrefix("Sandbox/App"));
        builder.Build().Run();
      }
      catch (Exception ex)
      {
        ex.Trace();
      }
    }
  }
}