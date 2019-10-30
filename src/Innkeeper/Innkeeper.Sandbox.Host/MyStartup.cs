using Innkeeper.Host;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innkeeper.Host.Core
{
  public class MyStartup
  {
    public MyStartup()
    {
    }

    public void ConfigureServices(IServiceCollection services)
    {
      // services.AddInnkeeper();
    }

    public void Configure(IApplicationBuilder app)
    {
      // app.UseInnkeeper();
    }
  }
}