using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Toolset;
using Toolset.Collections;

namespace Paper.Media2
{
  public class Entity : HashMap, IMediaObject
  {
    public Uid Uid { get; set; }

    public string[] Class { get; set; }

    public string Title { get; set; }

    public static implicit operator Uid(Entity media)
    {
      return media.Uid;
    }
  }
}