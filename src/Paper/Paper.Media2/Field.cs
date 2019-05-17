using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Toolset;
using Toolset.Collections;

namespace Paper.Media2
{
  public class Field : IMediaObject
  {
    public Uid Uid { get; set; }

    public string Name { get; set; }

    public string DataType { get; set; }

    public string ComponentType { get; set; }

    public Var Value { get; set; }

    public static implicit operator Uid(Field media)
    {
      return media.Uid;
    }
  }
}