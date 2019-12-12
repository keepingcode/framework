using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Media
{
  public interface IFormData : IMediaObject, IValueCollection
  {
    Record Form { get; set; }

    Record[] Records { get; set; }
  }
}
