using System;
using System.Collections.Generic;
using System.Text;
using Paper.Commons;

namespace Paper.Extensions.Site
{
  public class SiteMapCatalog : Catalog<ISiteMap>, ISiteMapCatalog
  {
    public SiteMapCatalog()
      : base(item => item.Href)
    {
    }
  }
}
