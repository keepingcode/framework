using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Paper.Browser.Web;
using Paper.Media;
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
        var loader = new ResourceLoader();

        foreach (var path in loader.ResourcePaths)
        {
          Debug.WriteLine(path);
        }

        Debug.WriteLine("- - -");

        var ret = loader.FindResources(@"??d.file");
        Debug.WriteLine(ret.Status); 
        foreach (var path in ret.Value)
        {
          var content = loader.LoadResourceAsText(path);
          Debug.Write(path);
          Debug.Write(": ");
          Debug.Write(content.Status);
          Debug.Write(": ");
          Debug.WriteLine(content);
        }
      }
      catch (Exception ex)
      {
        ex.Trace();
      }
    }
  }
}