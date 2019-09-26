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
  public class Startup
  {
    private IHostingEnvironment environment;
    private ILoggerFactory loggerFactory;

    public Startup(IHostingEnvironment environment, ILoggerFactory loggerFactory)
    {
      this.environment = environment;
      this.loggerFactory = loggerFactory;
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddInnkeeper();
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseHsts();
        app.UseHttpsRedirection();
      }

      app.UseInnkeeper();

      app.UseDefaultFiles();
      app.UseStaticFiles();
      app.UseDirectoryBrowser();
      app.UseCookiePolicy();
    }
  }
}
