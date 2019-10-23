using Paper.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Browser
{
  public interface IPaperCatalog
  {
    Type GetPaperType(PaperId paperId);
  }
}
