using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Media
{
  public interface ISentData : IMediaObject, IValueCollection
  {
    Record Error { get; set; }

    Record[] Records { get; set; }
  }
}