using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolset.Collections;

namespace Paper.Media
{
  public class Link
  {
    public string Title { get; set; }

    public Collection<string> Rel { get; set; }

    public string Href { get; set; }
  }
}
