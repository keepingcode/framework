using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Rendering
{
  public interface IPaperDescriptor
  {
    string Catalog { get; }

    string Paper { get; }

    string Title { get; }

    Type PaperType { get; }
  }
}
