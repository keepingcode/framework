using Innkeeper.Host;
using Paper.Browser;
using Paper.Design;
using System;
using System.Collections.Generic;
using System.Text;
using Toolset;
using Toolset.Reflection;

namespace Paper.Host
{
  [Expose]
  class PaperInnkeeperModule : IInnkeeperModule
  {
    public void ConfigureServices(IObjectFactoryBuilder builder)
    {
      var paperCatalog = new PaperCatalog();
      paperCatalog.Fill();

      Console.WriteLine("--papers--");
      foreach (var key in paperCatalog.Keys)
      {
        Console.WriteLine($"{key} => /Paper/Api/1/Modules/{key.Module}/Papers/{key.Schema}");
      }
      Console.WriteLine("----");

      builder.AddSingleton<IPaperCatalog>(paperCatalog);
    }
  }
}
