using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Media
{
  public interface ISentData : IMediaObject, IValueCollection
  {
    PropertyCollection Error { get; set; }

    PropertyCollection Records { get; set; }
  }
}