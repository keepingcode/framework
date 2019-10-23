using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Design
{
  [AttributeUsage(AttributeTargets.Class)]
  public class PaperAttribute : Attribute
  {
    public PaperAttribute()
    {
    }

    public PaperAttribute(string schema)
    {
      this.Schema = schema;
    }

    public string Module { get; set; }

    public string Schema { get; set; }
  }
}
