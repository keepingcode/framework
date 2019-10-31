using Innkeeper.Host;
using System;
using System.Collections.Generic;
using System.Text;
using Toolset;

namespace Paper
{
  [Expose]
  class PaperModule : IModule
  {
    public void Install(IObjectFactoryBuilder builder)
    {
      // var paperCatalog = new PaperCatalog();
      // paperCatalog.Fill();
      // 
      // Console.WriteLine("--papers--");
      // foreach (var key in paperCatalog.Keys)
      // {
      //   Console.WriteLine($"{key} => /Paper/Api/1/Modules/{key.Module}/Papers/{key.Schema}");
      // }
      // Console.WriteLine("----");
      // 
      // builder.AddSingleton<IPaperCatalog>(paperCatalog);
    }
  }
}
