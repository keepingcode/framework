﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Paper.Media.Design;
using Paper.Media.Serialization;
using Toolset;
using Toolset.Collections;
using Toolset.Reflection;

namespace Paper.Media
{
  [DataContract(Namespace = Namespaces.Default, Name = "Payload")]
  public class Payload : IMediaObject
  {
    [DataMember]
    public PropertyMap Error { get; set; }

    [DataMember]
    public PropertyMap Form { get; set; }

    [DataMember]
    public PropertyMap Record { get; set; }

    [DataMember]
    public RecordCollection Records { get; set; }

    Payload IMediaObject.GetPayload() => this;

    [CollectionDataContract(Namespace = Namespaces.Default, Name = "Records", ItemName = "Record")]
    public class RecordCollection : List<PropertyMap>
    {
    }

    #region Operações extras

    public Entity ToEntity()
    {
      var entity = new Entity();

      var hasError = (Error != null);
      var hasForm = (Form != null);
      var hasRecord = (Record != null);
      var hasRecords = (Records != null);

      var allRecords = Enumerable.Empty<PropertyMap>();

      if (hasError)
      {
        entity.AddClass(ClassNames.Status, ClassNames.Error);
        CopyMapToEntity(Error, entity);
      }
      else
      {
        if (hasForm)
        {
          entity.AddClass(ClassNames.Form);
          CopyMapToEntity(Form, entity);

          if (hasRecord)
          {
            allRecords = allRecords.Append(Record);
          }
        }
        else if (hasRecord)
        {
          entity.AddClass(ClassNames.Record);
          CopyMapToEntity(Record, entity);
        }

        if (hasRecords)
        {
          allRecords = allRecords.Concat(Records);
        }

        foreach (var record in allRecords)
        {
          var child = new Entity();
          child.AddClass(ClassNames.Record);
          if (hasForm)
          {
            child.AddRel(ClassNames.Form);
          }
          CopyMapToEntity(record, child);
          entity.AddEntity(child);
        }
      }

      return entity;
    }

    public static Payload FromGraph(object graph)
    {
      if (graph is Entity entity)
        return FromEntity(entity);

      var payload = new Payload();

      if (graph is ICollection list)
      {
        payload.Records = new RecordCollection();
        foreach (var item in list)
        {
          var map = new PropertyMap();
          CopyGraphToMap(item, map);
          payload.Records.Add(map);
        }
      }
      else
      {
        payload.Record = new PropertyMap();
        CopyGraphToMap(graph, payload.Record);
      }

      return payload;
    }

    public static Payload FromEntity(Entity entity)
    {
      var payload = new Payload();
      var children = entity.Children().Where(e => e.Class.Has(ClassNames.Record));

      var hasError = entity.Class.Has(ClassNames.Error);
      var hasForm = entity.Class.Has(ClassNames.Form);
      var hasRecord = entity.Class.Has(ClassNames.Record);

      if (hasError)
      {
        payload.Error = new PropertyMap();
        CopyEntityToMap(entity, payload.Error);
      }
      else
      {
        if (hasForm)
        {
          payload.Form = new PropertyMap();
          CopyEntityToMap(entity, payload.Form);

          if (hasRecord)
          {
            children = entity.AsSingle().Concat(children);
          }
        }
        else if (hasRecord)
        {
          payload.Record = new PropertyMap();
          CopyEntityToMap(entity, payload.Record);
        }

        if (children.Any())
        {
          payload.Records = new RecordCollection();
          foreach (var child in children)
          {
            var map = new PropertyMap();
            CopyEntityToMap(child, map);
            payload.Records.Add(map);
          }
        }
      }
      return payload;
    }

    private static void CopyEntityToMap(Entity entity, PropertyMap map)
    {
      var @class = entity.Class?.FirstOrDefault(x => char.IsUpper(x.First()));
      if (@class != null)
      {
        map.Add("@class", @class);
      }
      if (entity.Properties is PropertyMap properties)
      {
        foreach (var property in properties.Where(x => !x.Key.StartsWith("__")))
        {
          map.Add(property.Key, property.Value);
        }
      }
    }

    private static void CopyMapToEntity(PropertyMap map, Entity entity)
    {
      if (map["@class"] is string @class)
      {
        entity.AddClass(@class);
      }
      var properties = map.Where(x => !x.Key.EqualsAnyIgnoreCase("@class"));
      entity.AddProperties(properties);
    }

    private static void CopyGraphToMap(object graph, PropertyMap map)
    {
      var @class = graph.GetType().Name;
      map.Add("@class", @class);
      foreach (var property in graph._GetPropertyNames())
      {
        var value = graph._Get(property);
        map.Add(property, value);
      }
    }

    #endregion
  }
}