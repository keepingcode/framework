using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Paper.Sandbox.Host.Abstract;
using Paper.Sandbox.Host.Demo.Papers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Toolset;

namespace Paper.Sandbox.Host
{
  static class Program
  {
    static void Main(string[] args)
    {
      try
      {
        var extractor = new PaperSchemaExtractor();
        var schema = extractor.ExtractSchema(typeof(UserPaper));

        //InnkeeperWebHost.Run(app =>
        //{
        //  app.Port = 9090;
        //  app.Name = "PaperSandbox";
        //}, args);
      }
      catch (Exception ex)
      {
        ex.Trace();
      }
    }
  }
}