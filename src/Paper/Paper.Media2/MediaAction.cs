using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Toolset;
using Toolset.Collections;

namespace Paper.Media2
{
  public class MediaAction : IMediaObject
  {
    public Uid Uid { get; set; }

    public string Name { get; set; }

    public static implicit operator Uid(MediaAction media)
    {
      return media.Uid;
    }
  }
}