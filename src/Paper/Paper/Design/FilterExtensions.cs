using System;
using System.Collections.Generic;
using System.Text;
using Toolset;

namespace Paper.Design
{
  public static class FilterExtensions
  {
    public static UriString CreateUri(this IFilter filter, UriString baseUri)
    {
      // TODO: Implementar a renderização do filtro na URI
      return baseUri;
    }
  }
}
