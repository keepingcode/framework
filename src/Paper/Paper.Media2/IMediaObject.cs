using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Toolset;
using Toolset.Collections;

namespace Paper.Media2
{
  public interface IMediaObject
  {
    Uid Uid { get; set; }
  }
}