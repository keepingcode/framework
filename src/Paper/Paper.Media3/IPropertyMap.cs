using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Paper.Media3.Design;
using Paper.Media3.Serialization;
using Toolset.Reflection;

namespace Paper.Media3
{
  public interface IPropertyMap
  {
    IEnumerable<string> PropertyNames { get; }

    object this[string propertyName] { get; set; }
  }
}

