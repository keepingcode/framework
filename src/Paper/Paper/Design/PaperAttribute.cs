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
      this.Name = schema;
    }

    public string Catalog { get; set; }

    public string Name { get; set; }

    public string Title { get; set; }
  }
}
