using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Toolset;
using Toolset.Collections;

namespace Paper.Media3
{
  public class Payload
  {
    public PropertyMap Error { get; set; }

    public PropertyMap Form { get; set; }

    public PropertyMap Record { get; set; }

    public RecordCollection Records { get; set; }

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
        entity.Class.AddMany(ClassNames.Status, ClassNames.Error);
        CopyMapToEntity(Error, entity);
      }
      else
      {
        if (hasForm)
        {
          entity.Class.Add(ClassNames.Form);
          CopyMapToEntity(Form, entity);

          if (hasRecord)
          {
            allRecords = allRecords.Append(Record);
          }
        }
        else if (hasRecord)
        {
          entity.Class.Add(ClassNames.Record);
          CopyMapToEntity(Record, entity);
        }

        if (hasRecords)
        {
          allRecords = allRecords.Concat(Records);
        }

        foreach (var record in allRecords)
        {
          var child = new Entity();
          child.Class.Add(ClassNames.Record);
          if (hasForm)
          {
            child.Rel.Add(ClassNames.Form);
          }
          CopyMapToEntity(record, child);
          entity.Entities.Add(child);
        }
      }

      return entity;
    }

    public static Payload FromEntity(Entity entity)
    {
      var payload = new Payload();
      var children = entity.Entities.Where(e => e.Class.Has(ClassNames.Record));

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

    private static void CopyEntityToMap(IEntity entity, PropertyMap map)
    {
      var @class = entity.Class?.FirstOrDefault(x => char.IsUpper(x.First()));
      if (@class != null)
      {
        map["@class"] = @class;
      }
      if (entity.Properties is PropertyMap properties)
      {
        foreach (var property in properties.Where(x => !x.Key.StartsWith("__")))
        {
          map[property.Key] = property.Value;
        }
      }
    }

    private static void CopyMapToEntity(PropertyMap map, IEntity entity)
    {
      if (map["@class"] is string @class)
      {
        entity.Class.Add(@class);
      }
      var properties = map.Where(x => !x.Key.EqualsAnyIgnoreCase("@class"));
      foreach (var entry in properties)
      {
        entity.Properties[entry.Key] = entry.Value;
      }
    }

    public class RecordCollection : List<PropertyMap>
    {
    }
  }
}