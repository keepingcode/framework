using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Rendering
{
  public interface IPaperDescriptor
  {
    string Module { get; }

    string Schema { get; }

    Type PaperType { get; }
  }
}
