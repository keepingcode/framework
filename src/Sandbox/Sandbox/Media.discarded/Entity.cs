using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolset.Collections;

namespace Paper.Media
{
  public class Entity
  {
    public string Title { get; set; }

    public Collection<string> Classes { get; set; }

    public Collection<string> Relations { get; set; }

    public Collection<Property> Payload { get; set; }

    public Collection<Property> Metas { get; set; }

    public Collection<Entity> Entities { get; set; }

    public Collection<Link> Links { get; set; }
  }
}
