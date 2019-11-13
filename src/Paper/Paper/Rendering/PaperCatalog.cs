﻿using Paper.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolset;
using Toolset.Collections;
using Toolset.Reflection;

namespace Paper.Rendering
{
  class PaperCatalog : IPaperCatalog
  {
    private HashMap<IPaperDescriptor> descriptorMap = new HashMap<IPaperDescriptor>();

    public void MapPaperDescriptor(IPaperDescriptor descriptor)
    {
      var id = $"{descriptor.Module}/{descriptor.Schema}";
      descriptorMap[id] = descriptor;
    }

    public IPaperDescriptor MapPaperDescriptor(Type paperType)
    {
      var descriptor = new PaperDescriptor
      {
        Module = PaperDescriptor.IdentifyModule(paperType),
        Schema = PaperDescriptor.IdentifySchema(paperType),
        PaperType = paperType
      };
      var id = $"{descriptor.Module}/{descriptor.Schema}";
      descriptorMap[id] = descriptor;
      return descriptor;
    }

    public IPaperDescriptor MapPaperDescriptor(string module, string schema, Type paperType)
    {
      var descriptor = new PaperDescriptor
      {
        Module = module,
        Schema = schema,
        PaperType = paperType
      };
      var id = $"{descriptor.Module}/{descriptor.Schema}";
      descriptorMap[id] = descriptor;
      return descriptor;
    }

    public ICollection<IPaperDescriptor> FindPaperDescriptor(Type paperType)
    {
      var descriptors = (
        from entry in descriptorMap
        where entry.Value.PaperType == paperType
        select entry.Value
      ).ToArray();
      return descriptors;
    }

    public ICollection<IPaperDescriptor> FindPaperDescriptor(string module)
    {
      var prefix = $"{module}/";
      var descriptors = (
        from entry in descriptorMap
        where entry.Key.StartsWith(prefix)
        select entry.Value
      ).ToArray();
      return descriptors;
    }

    public IPaperDescriptor FindPaperDescriptor(string module, string schema)
    {
      var id = $"{module}/{schema}";
      var descriptor = descriptorMap[id];
      return descriptor;
    }
  }
}