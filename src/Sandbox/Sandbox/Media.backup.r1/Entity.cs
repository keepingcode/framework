using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolset.Collections;

namespace Paper.Media
{
  public class Entity : IMediaObject
  {
    public string Title { get; set; }

    public NameCollection Classes { get; set; }

    public NameCollection Relations { get; set; }

    public PropertyCollection DataProperties { get; set; }

    public PropertyCollection MetaProperties { get; set; }

    public Collection<Entity> Entities { get; set; }

    public Collection<Link> Links { get; set; }

    public Payload ExtractPayload()
    {
      var payload = new Payload();

      if (Classes?.Contains(ClassNames.Error) == true)
      {
        payload.Error = new Record
        {
          Classes = GetUserClasses(this.Classes),
          Properties = new PropertyCollection(DataProperties.Select(x => x.Clone()))
        };
      }
      else
      {
        if (Classes?.Contains(ClassNames.Form) == true)
        {
          payload.Form = new Record
          {
            Classes = GetUserClasses(this.Classes),
            Properties = new PropertyCollection(DataProperties.Select(x => x.Clone()))
          };
        }

        payload.Records = ExtractRecords(this).ToArray();
      }

      return payload;
    }

    private IEnumerable<Record> ExtractRecords(Entity entity)
    {
      throw new NotImplementedException();
      //var isRecord = entity?.Classes?.Contains(ClassNames.Record) == true;
      //if (!isRecord)
      //  yield break;

      //var classes = GetUserClasses(entity.Classes);
      //var properties = new PropertyCollection(entity.DataProperties?.Select(x => x.Clone()));

      //var record = new Record
      //{
      //  Classes = classes,
      //  Properties = properties
      //};

      //var children = entity.Entities?.Where(x => x.Classes?.Contains(ClassNames.Record) == true);
      //if (children != null)
      //{
      //  var records = new List<Record>();
      //  foreach (var child in children)
      //  {
      //    var entities = ExtractRecords(child);
      //    records.AddRange(entities);
      //  }
      //  record.Properties = new PropertyCollection(records);
      //}

      //yield record;
    }

    private NameCollection GetUserClasses(NameCollection classes)
    {
      var names = new NameCollection();
      if (classes != null)
      {
        names.AddMany(
          from @class in classes
          where char.IsUpper(@class.FirstOrDefault())
          select @class
        );
      }
      return names;
    }
  }
}
