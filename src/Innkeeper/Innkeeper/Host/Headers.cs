using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Innkeeper.Pipelines;
using Microsoft.AspNetCore.Http;
using Toolset;

namespace Innkeeper.Host
{
  internal class Headers : IHeaders
  {
    private IHeaderDictionary headers;

    public Headers(IHeaderDictionary headers)
    {
      this.headers = headers;
    }

    public int Count => headers.Count;

    public ICollection<string> Keys => this.headers.Keys;

    public string this[string key]
    {
      get => headers[key];
      set => headers[key] = value;
    }
  }
}
