
using System;
using System.Collections.Generic;
using System.Text;
using Paper.Rendering;
using Paper.Media;

namespace Paper.Extensions.Papers
{
  public interface IFormatter
  {
    void Format(IPaperContext context, IObjectFactory factory, Entity entity);
  }
}