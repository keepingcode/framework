﻿using Innkeeper.Host;
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
  public interface IPaperBuilder<THost>
  {
    IStatementBuilder<THost, TData> Get<TData>();
    IStatementBuilder<THost, TData> Get<TData>(Func<IPaperContext, THost, TData> dataFactory);
    IStatementBuilder<THost, TData> Post<TData>(Func<IPaperContext, THost, IMediaObject, TData> dataFactory);
    IPaperBlueprint BuildPaper();
  }
}