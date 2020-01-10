using System;
using System.Collections.Generic;
using System.Text;
using Toolset.Collections;

namespace Paper.Design
{
  public class Bulk<TForm, TRecord> : Collection<Bulk<TForm, TRecord>.Entry>
  {
    public class Entry
    {
      public TForm Form { get; set; }
      public TRecord Record { get; set; }
    }
  }
}