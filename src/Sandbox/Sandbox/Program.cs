using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        var json = @"
{
  ""name"": ""Field"",
  ""value"":
    {
      ""title"": ""Title"",
      ""value"": 10
    }
}
";

        using (var reader = new JsonReader(new StringReader(json)))
        using (var writer = new GraphWriter<Field>())
        {
          reader.CopyTo(writer);

          var user = writer.Graphs.FirstOrDefault();
          Debug.WriteLine(user.Name);
          Debug.WriteLine(user.Value);
        }

      }
      catch (Exception ex)
      {
        ex.Trace();
      }
    }
  }
}