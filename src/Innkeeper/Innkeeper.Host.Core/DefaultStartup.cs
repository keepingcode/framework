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
  internal class DefaultStartup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddPaper();
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseDirectoryBrowser();
      }
      // else
      // {
      //   app.UseHsts();
      //   app.UseHttpsRedirection();
      // }

      app.UsePaper();

      app.UseDefaultFiles();
      app.UseStaticFiles();
      app.UseCookiePolicy();
    }
  }
}