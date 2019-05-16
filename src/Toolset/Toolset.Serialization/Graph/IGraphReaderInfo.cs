﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Toolset.Serialization.Graph
{
  public interface IGraphReaderInfo
  {
    string GetLabel();
    string GetPropertyLabel(PropertyInfo property);
    IEnumerable<PropertyInfo> GetProperties();
  }
}
