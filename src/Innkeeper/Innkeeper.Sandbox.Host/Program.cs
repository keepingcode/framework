using Innkeeper.Host.Core;
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
      InnkeeperHost.Start(
        port: 9090,
        prefix: "/Sandbox/App",
        args: args
      );
    }
  }
}