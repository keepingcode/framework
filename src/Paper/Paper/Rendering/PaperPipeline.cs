using Innkeeper.Host;
using Innkeeper.Rest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Toolset;

namespace Paper.Rendering
{
  class PaperPipeline : RestPipeline
  {
    [Get("/")]
    async Task GetCatalog(int id)
    {
      await Res.SendAsync($"/id={id}");
    }

    [Get("/Papers")]
    async Task GetPapers()
    {
      await Res.SendAsync($"/Papers");
    }

    [Get("/Papers/{paper}")]
    async Task GetPaper(string paper)
    {
      await Res.SendAsync($"/Papers/{paper}");
    }

    [Get("/Papers/{paper}/Actions")]
    async Task GetActions(string paper)
    {
      await Res.SendAsync($"/Papers/{paper}/Actions");
    }

    [Get("/Papers/{paper}/Actions/{action}")]
    async Task GetActions(string paper, string action)
    {
      await Res.SendAsync($"/Papers/{paper}/Actions/{action}");
    }
  }
}
