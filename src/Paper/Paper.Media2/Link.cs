using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Toolset;
using Toolset.Collections;

namespace Paper.Media2
{
  public class Link : IMediaObject
  {
    public Uid Uid { get; set; }

    public string Name { get; set; }

    public string Href { get; set; }

    public static implicit operator Uid(Link media)
    {
      return media.Uid;
    }
  }
}