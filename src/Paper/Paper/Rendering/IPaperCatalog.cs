using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Paper.Rendering
{
  public interface IPaperCatalog
  {
    IPaperDescriptor FindPaperDescriptor(string module, string schema);

    ICollection<IPaperDescriptor> FindPaperDescriptor(string module);

    ICollection<IPaperDescriptor> FindPaperDescriptor(Type paperType);
  }
}
