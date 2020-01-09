using System;
using System.Collections.Generic;
using System.Text;
using Paper.Commons;

namespace Paper.Extensions.Site
{
  public class SiteMapAttribute : CatalogAttribute
  {
    public SiteMapAttribute(string collectionName)
      : base(collectionName)
    {
    }
  }
}