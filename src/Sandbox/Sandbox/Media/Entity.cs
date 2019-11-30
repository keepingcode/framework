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

    public Collection<string> Classes { get; set; }

    public Collection<string> Relations { get; set; }

    public PropertyCollection DataProperties { get; set; }

    public PropertyCollection MetaProperties { get; set; }

    public Collection<Entity> Entities { get; set; }

    public Collection<Link> Links { get; set; }

    public Payload ToPayload()
    {
      var payload = new Payload();

      if (Classes?.Contains(ClassNames.Error) == true)
      {
        payload.Error = new PropertyCollection(DataProperties);
        payload.Error.AddAt(0, "@class", (Text)ClassifyEntity(this, ClassNames.Error));
      }
      else
      {
        if (Classes?.Contains(ClassNames.Form) == true)
        {
          payload.Form = new PropertyCollection(DataProperties);
          payload.Form.AddAt(0, "@class", (Text)ClassifyEntity(this, ClassNames.Form));
        }

        payload.Records = new PropertyCollection();

        // TODO: o algoritmo deveria ser recursivo

        if (Classes?.Contains(ClassNames.Record) == true)
        {
          payload.Records.AddMany(DataProperties);
          payload.Records.AddAt(0, "@class", (Text)ClassifyEntity(this, ClassNames.Record));
        }

        if (Entities != null)
        {
          foreach (var entity in Entities)
          {
            if (entity.Classes?.Contains(ClassNames.Record) == true)
            {
              payload.Records.AddMany(entity.DataProperties);
              payload.Records.AddAt(0, "@class", (Text)ClassifyEntity(entity, ClassNames.Record));
            }
          }
        }
      }

      return payload;
    }

    private string ClassifyEntity(Entity source, string defaultValue)
    {
      var className = source.Classes?
        .Where(x => char.IsUpper(x.FirstOrDefault()))
        .FirstOrDefault();
      return className ?? defaultValue;
    }
  }
}
