using System;
using System.Collections.Generic;
using System.Text;
using Paper.Rendering;
using Paper.Media.Data;
using Toolset;
using Toolset.Collections;

namespace Paper.Extensions.Papers
{
  internal class PaperContext : IPaperContext
  {
    public PaperContext()
    {
      RenderContext = new RenderContext();
    }

    public PaperDescriptor Paper { get; set; }

    public string Path { get; set; }

    public string Action { get; set; }

    public Args Args { get; set; }

    public Request Request { get; set; }

    public Response Response { get; set; }

    public RenderContext RenderContext { get; }
  }
}