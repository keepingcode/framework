using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Media
{
  public interface IFormData : IMediaObject, IValueCollection
  {
    PropertyCollection Form { get; set; }

    PropertyCollection Records { get; set; }
  }
}
