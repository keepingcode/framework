using System;
using System.Collections.Generic;
using System.Text;
using Toolset.Collections;

namespace Paper.Design
{
  public class Mass<TForm, TRecord> : Collection<TRecord>
  {
    public TForm Form { get; set; }
  }
}