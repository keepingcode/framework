﻿using Paper.Media;
using Paper.Media.Serialization;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Toolset;
using Toolset.Collections;
using Toolset.Net;
using Toolset.Xml;

namespace Sandbo
{
  public static class Program
  {
    public static void Main(string[] commandArgs)
    {
      try
      {
        var record = new Record();
        record.Add(new Property("title", "My Title"));
        record.Add(new Property("header", "My Header"));

        var link = new Link();
        link.Add(new Property("title", "My Title"));
        link.Add(new Property("href", "My #ref"));
        link.Add(new Property("header", "My Header"));
        link.Add(new Class("header"));
        link.Add(new Rel("header"));
        record.Add(link);

        var file = new ResourceRef();
        file.Add(new Property("title", "My Title"));
        file.Add(new Property("href", "My #ref"));
        file.Add(new Property("header", "My Header"));
        file.Add(new Class("header"));
        file.Add(new Rel("header"));
        record.Add(file);
        
        record.Add(new Property("__meta", Value.CreateObject(new
        {
          Tables = new
          {
            Headers = new VName[] {
              "Id",
              "Name"
            }
          }
        })));


        var p1 = record.GetProperty<VArray>("__meta.tables.headers");
        record.SetProperty("__meta.tables.headers.id", new[] { 10 });
        var p2 = record.GetProperty<VArray>("__meta.tables.headers.id");
        
        var serializer = new MediaSerializer(MimeType.JsonSiren);
        var step1 = serializer.Serialize(record);

        var target = serializer.Deserialize(step1);
        var step2 = serializer.Serialize(record);


        Debug.WriteLine(Json.Beautify(step1));
        Debug.WriteLine(Json.Beautify(step2));
      }
      catch (Exception ex)
      {
        ex.Trace();
      }
    }
  }
}