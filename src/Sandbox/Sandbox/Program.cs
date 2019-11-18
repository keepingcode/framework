using Innkeeper.Rest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Toolset;
using Toolset.Collections;
using Toolset.Xml;

namespace Sandbox
{
  public static class Program
  {
    public static void Main(string[] commandArgs)
    {
      try
      {
        var url = new Url();

        url.Protocol = "http";
        url.Host = "10.0.0.1";
        url.Port = 090;
        url.User = "xyz";
        url.Pass = "123";
        url.Path = "/Api/1/App";

        url.Args["page"] = new Var(new Toolset.Range(10, 30));
        //url.Args["oneShot"] = new Var(true);
        //url.Args["search"] = new Var("Tananana");
        url.Args["ids"] = new Var(new[] { 1, 2, 3 });

        //Debug.WriteLine(url.Clone().Combine("?page.min=5&ids[]=33"));
        //Debug.WriteLine(url.Clone().Combine("?page.min=5&ids[]=33"));
        //Debug.WriteLine(url.Clone().Replace("?page.min=5&ids[]=33"));


        Debug.WriteLine(url);

        Debug.WriteLine(url.Clone().Combine(".././Xyz"));
        Debug.WriteLine(url.Clone().Combine(".././../Xyz"));
        Debug.WriteLine(url.Clone().Combine(".././../../Xyz"));

        Debug.WriteLine(url.Clone().Append("/One/?x=1").Combine(".././Xyz?o=p"));
        Debug.WriteLine(url.Clone().Append("/One/?x=1").Combine(".././../Xyz?o=p"));
        Debug.WriteLine(url.Clone().Append("/One/?x=1").Combine(".././../../Xyz?o=p"));

        //var other =
        //  url
        //    .Clone()
        //    .ClearArgs()
        //    .Append("/Talz")
        //    .Combine("/Talz")
        //    .Append("?a[]=1")
        //    .Combine("?a[]=2")
        //    .Replace("*//local:*/");

      }
      catch (Exception ex)
      {
        ex.Trace();
      }
    }
  }
}