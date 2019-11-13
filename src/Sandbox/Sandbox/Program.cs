using Innkeeper.Rest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Toolset;
using Toolset.Collections;
using Toolset.Xml;

namespace Sandbox
{
  public interface IUrl : ICloneable
  {
    string Protocol { get; set; }

    string Host { get; set; }

    int? Port { get; set; }

    string Path { get; set; }

    HashMap<Var> Args { get; }

    IUrl Combine(params string[] fragments);

    IUrl Append(params string[] fragments);

    IUrl Replace(params string[] fragments);

    IUrl ClearArgs();

    IUrl SetArg(string argName, object value);

    IUrl SetArgs(params object[] argValuePairs);

    new IUrl Clone();
  }

  public static class Program
  {
    public static void Main(string[] commandArgs)
    {
      try
      {
        IUrl url = null;

        var other =
          url
            .Clone()
            .ClearArgs()
            .Append("/Talz")
            .Combine("/Talz")
            .Append("?a[]=1")
            .Combine("?a[]=2")
            .Replace("*//local:*/");

      }
      catch (Exception ex)
      {
        ex.Trace();
      }
    }
  }
}