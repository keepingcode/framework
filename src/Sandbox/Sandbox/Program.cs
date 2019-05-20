using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Paper.Media3;
using Toolset;
using Toolset.Collections;
using Toolset.Serialization;
using Toolset.Serialization.Graph;
using Toolset.Serialization.Json;

namespace Sandbox
{
  public static class Program
  {
    public static void Main(string[] args)
    {
      try
      {
        var entity = new Entity();
        entity.Properties["__headers"] = new ValueMap();
        ((ValueMap)entity.Properties["__headers"])["data"] = new ValueCollection();
        ((ValueCollection)((ValueMap)entity.Properties["__headers"])["data"]).Add("id");
        ((ValueCollection)((ValueMap)entity.Properties["__headers"])["data"]).Add("name");

        Debug.WriteLine(entity);
      }
      catch (Exception ex)
      {
        ex.Trace();
      }
    }
  }
}