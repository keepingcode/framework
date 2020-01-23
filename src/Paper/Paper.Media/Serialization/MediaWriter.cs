using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Toolset;
using Toolset.Collections;
using Toolset.Serialization;

namespace Paper.Media.Serialization
{
  internal class MediaWriter
  {
    private Map<Type, string[]> reservedKeyMap = new Map<Type, string[]>();

    public void Write(IMedia media, Writer writer)
    {
      writer.WriteDocumentStart();
      WriteMedia(media, writer);
      writer.WriteDocumentEnd();
      writer.Flush();
    }

    private ICollection<string> GetReservedPropertyNames(object @object)
    {
      var type = @object.GetType();
      if (!reservedKeyMap.ContainsKey(type))
      {
        var keys = (
          from property in type.GetProperties()
          select property.Name
        ).ToArray();
        reservedKeyMap[type] = keys;
      }
      return reservedKeyMap[type];
    }

    private void WriteMedia(IMedia media, Writer writer)
    {
      writer.WriteObjectStart("Entity");

      WriteClasses(media.OfType<Class>(), writer);
      WriteRels(media.OfType<Rel>(), writer);

      WriteProperties(media, writer);

      var entities =
        from child in media.OfType<IMedia>()
        let priority =
          child is Record ? 1 : child is Field ? 2 : child is Action ? 3 : 4
        orderby priority
        select child;
      WriteMedias(entities, writer);

      WriteLinks(media.OfType<Link>(), writer);

      writer.WriteObjectEnd();
    }

    private void WriteMedias(IEnumerable<IMedia> entities, Writer writer)
    {
      if (!entities.Any())
        return;

      writer.WritePropertyStart("Entities");
      writer.WriteCollectionStart();
      foreach (var entity in entities)
      {
        WriteMedia(entity, writer);
      }
      writer.WriteCollectionEnd();
      writer.WritePropertyEnd();
    }

    private void WriteClasses(IEnumerable<Class> classes, Writer writer)
    {
      if (!classes.Any())
        return;

      classes = classes.OrderBy(x => x.IsMetaTag);

      writer.WritePropertyStart("Class");
      writer.WriteCollectionStart();
      foreach (var @class in classes)
      {
        writer.WriteValue(@class);
      }
      writer.WriteCollectionEnd();
      writer.WritePropertyEnd();
    }

    private void WriteRels(IEnumerable<Rel> rels, Writer writer)
    {
      if (!rels.Any())
        return;

      rels = rels.OrderBy(x => x.IsMetaTag);

      writer.WritePropertyStart("Rel");
      writer.WriteCollectionStart();
      foreach (var rel in rels)
      {
        writer.WriteValue(rel);
      }
      writer.WriteCollectionEnd();
      writer.WritePropertyEnd();
    }

    private void WriteProperties(ICollection<INode> target, Writer writer)
    {
      var keys = GetReservedPropertyNames(target);

      var fields =
        from key in keys
        join property in target.OfType<Property>()
          on key equals property.Name
        select property;

      var properties =
        from property in target.OfType<Property>()
        where !property.Hidden
        select property;

      foreach (var field in fields)
      {
        WriteProperty(field, writer);
      }

      if (properties.Any())
      {
        writer.WritePropertyStart("Properties");
        writer.WriteObjectStart("Properties");
        foreach (var property in properties)
        {
          WriteProperty(property, writer);
        }
        writer.WriteObjectEnd();
        writer.WritePropertyEnd();
      }
    }

    private void WriteProperty(Property property, Writer writer)
    {
      writer.WritePropertyStart(property.Name);
      WriteValue(property.Value, writer);
      writer.WritePropertyEnd();
    }

    private void WriteValue(IValue value, Writer writer)
    {
      if (value == null)
      {
        writer.WriteValue(null);
      }
      else if (value is IEnumerable<Property> properties)
      {
        writer.WriteObjectStart();
        foreach (var property in properties)
        {
          WriteProperty(property, writer);
        }
        writer.WriteObjectEnd();
      }
      else if (value is VArray array)
      {
        writer.WriteCollectionStart();
        foreach (var item in array)
        {
          WriteValue(item, writer);
        }
        writer.WriteCollectionEnd();
      }
      else if (value is VString @string)
      {
        var text = @string.Value;
        if (@string.IsCaseVariant)
        {
          text = text.ChangeCase(writer.Settings.TextCase);
        }
        writer.WriteValue(text);
      }
      else
      {
        writer.WriteValue(value.Value);
      }
    }

    private void WriteLinks(IEnumerable<Link> links, Writer writer)
    {
      if (!links.Any())
        return;

      writer.WritePropertyStart("Links");
      writer.WriteCollectionStart();

      foreach (var link in links)
      {
        writer.WriteObjectStart("Link");

        WriteClasses(link.OfType<Class>(), writer);
        WriteRels(link.OfType<Rel>(), writer);

        WriteProperties(link, writer);

        writer.WriteObjectEnd();
      }

      writer.WriteCollectionEnd();
      writer.WritePropertyEnd();
    }
  }
}
