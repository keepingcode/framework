using System;
using System.Collections.Generic;
using System.Text;
using Paper.Commons;

namespace Paper.Extensions.Papers
{
  public interface IPaperCatalog : ICatalog<PaperDescriptor>
  {
    //PaperDescriptor FindByType(Type type);
    //PaperDescriptor FindByType<T>();
  }
}