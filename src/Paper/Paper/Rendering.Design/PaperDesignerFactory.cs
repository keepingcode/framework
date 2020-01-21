using Innkeeper.Host;
using Innkeeper.Rest;
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
  public class PaperDesignerFactory : IPaperDesignerFactory
  {
    private PaperDesignerFactory()
    {
    }

    public static PaperDesignerFactory Create()
    {
      return new PaperDesignerFactory();
    }

    public IPaperDesigner<THost> CreatePaperBuilder<THost>(PaperInfo info, Func<IPaperContext, THost> hostFactory)
    {
      var builder = new PaperDesigner<THost>(info, hostFactory);
      return builder;
    }
  }
}