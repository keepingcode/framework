using Innkeeper.Host;
using Innkeeper.Rest;
using Paper.Design;
using Paper.Media;
using Paper.Media.Design;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Toolset;
using Toolset.Collections;
using Toolset.Reflection;
using Toolset.Xml;

namespace Paper.Rendering.Design
{
  public class PaperBuilderFactory : IPaperBuilderFactory
  {
    private PaperBuilderFactory()
    {
    }

    public static PaperBuilderFactory Create()
    {
      return new PaperBuilderFactory();
    }

    public IPaperBuilder<THost> CreatePaperBuilder<THost>(PaperInfo info, Func<IPaperContext, THost> hostFactory)
    {
      var builder = new PaperBuilder<THost>(info, hostFactory);
      return builder;
    }
  }
}