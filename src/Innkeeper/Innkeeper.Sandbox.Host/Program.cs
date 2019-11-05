using Innkeeper.Host;
using Innkeeper.Host.Core;
using Innkeeper.Rest;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Toolset;

namespace Innkeeper.Sandbox.Host
{
  [Expose, Route("/Sandbox")]
  public class MyRestPipeline : RestPipeline
  {
    [Get("{*path}")]
    async Task GetUsers(string path)
    {
      await Res.SendAsync($"Hello, I couldn`t reconize you {path}");
    }

    [Get("/Users")]
    public async Task GetUsers(int id)
    {
      await Res.SendAsync($"Hello, all of you users!");
    }

    [Get("/Users/{id}")]
    public async Task GetUserAsync(int id)
    {
      await Res.SendAsync($"Hello, user {id}!");
    }
  }

  static class Program
  {
    static void Main(string[] args)
    {
      try
      {
        InnkeeperWebHost.Run(app => app.Port = 9090, args);
      }
      catch (Exception ex)
      {
        ex.Trace();
      }
    }
  }
}