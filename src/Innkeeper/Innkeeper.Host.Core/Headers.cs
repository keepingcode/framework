using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Innkeeper.Host;
using Microsoft.AspNetCore.Http;
using Toolset;
using Toolset.Net;

namespace Innkeeper.Host.Core
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

    public string this[Header key]
    {
      get => headers[key.GetName()];
      set => headers[key.GetName()] = value;
    }
  }
}
