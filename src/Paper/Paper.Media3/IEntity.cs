using System;
using System.Collections.Generic;
using System.Text;
using Toolset.Collections;

namespace Paper.Media3
{
  public interface IEntity
  {
    NameCollection Class { get; set; }

    string Title { get; set; }

    NameCollection Rel { get; set; }

    EntityCollection Entities { get; set; }

    LinkCollection Links { get; set; }
  }
}
