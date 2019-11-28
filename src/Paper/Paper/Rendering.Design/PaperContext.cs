using Innkeeper.Host;
using Innkeeper.Rest;
using Paper.Media;
using Paper.Media.Design;
using Paper.Media.Serialization;
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
  public class PaperContext : IPaperContext
  {
    private IMap<string, Var> _args;

    public string Verb { get; set; }

    public IMap<string, Var> Args
    {
      get => _args ?? (_args = new HashMap<Var>());
      set => _args = value;
    }

    public IMap<string, object> Cache { get; } = new HashMap<object>();

    public IDataReader IncomingData { get; set; }

    public IDataWriter OutgoingData { get; set; }
  }
}