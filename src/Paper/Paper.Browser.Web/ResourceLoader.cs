using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Toolset;

namespace Paper.Browser.Web
{
  public class ResourceLoader
  {
    public Ret LoadResource(string resourcePath, Stream output)
    {
      return Ret.NotFound(resourcePath);
    }

    public Ret LoadResource(string resourcePath, TextWriter writer)
    {
      return Ret.NotFound(resourcePath);
    }
  }
}
