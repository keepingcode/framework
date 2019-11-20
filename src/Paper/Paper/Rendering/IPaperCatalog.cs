using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Paper.Rendering
{
  public interface IPaperCatalog
  {
    IPaperDescriptor FindPaperDescriptor(string catalog, string paper);

    ICollection<IPaperDescriptor> FindPaperDescriptor(string catalog);

    ICollection<IPaperDescriptor> FindPaperDescriptor(Type paperType);
  }
}
