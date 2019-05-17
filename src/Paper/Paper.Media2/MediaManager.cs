using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolset.Collections;

namespace Paper.Media2
{
  public class MediaManager : IMediaManager
  {
    private readonly Map<Uid, IMediaObject> cache = new Map<Uid, IMediaObject>();
    private readonly Map<Uid, List<Uid>> relationshipCache = new Map<Uid, List<Uid>>();
    private readonly List<Uid> childrenCache = new List<Uid>();
    private readonly Map<Uid, List<string>> tagCache = new Map<Uid, List<string>>();

    public T Create<T>()
      where T : class, IMediaObject, new()
    {
      var @object = new T();
      Add(@object);
      return @object;
    }

    public Uid Add<T>(T media)
      where T : class, IMediaObject
    {
      if (media.Uid == null)
      {
        media.Uid = new Uid();
      }
      cache.Add(media.Uid, media);
      return media.Uid;
    }

    public T Get<T>(Uid uid)
      where T : class, IMediaObject
    {
      return (T)cache[uid];
    }

    public IEnumerable<T> List<T>()
      where T : class, IMediaObject
    {
      return cache.Values.OfType<T>();
    }

    public IEnumerable<T> List<T>(Uid parent)
      where T : class, IMediaObject
    {
      IEnumerable<Uid> uids;

      if (parent == null)
      {
        uids = cache.Keys.Except(childrenCache);
      }
      else
      {
        uids = relationshipCache[parent] ?? Enumerable.Empty<Uid>();
      }

      return
        from uid in uids
        let media = cache[uid]
        where media is T
        select (T)media;
    }

    public void Bind(Uid parent, params Uid[] children)
    {
      Bind(parent, children.AsEnumerable<Uid>());
    }

    public void Bind(Uid parent, IEnumerable<Uid> children)
    {
      var relationships = relationshipCache[parent];
      if (relationships == null)
      {
        relationshipCache[parent] = relationships = new List<Uid>();
      }
      var newRelations = children.Except(relationships).ToArray();
      relationships.AddRange(newRelations);

      var newChildren = children.Except(childrenCache).ToArray();
      childrenCache.AddRange(newChildren);
    }

    public void Tag(Uid parent, params Rel[] relations)
    {
      Tag(parent, relations.Select(x => x.GetName()));
    }

    public void Tag(Uid parent, IEnumerable<Rel> relations)
    {
      Tag(parent, relations.Select(x => x.GetName()));
    }

    public void Tag(Uid parent, params string[] relations)
    {
      Tag(parent, relations.AsEnumerable<string>());
    }

    public void Tag(Uid parent, IEnumerable<string> relations)
    {
      var tags = tagCache[parent];
      if (tags == null)
      {
        tagCache[parent] = tags = new List<string>();
      }
      var newers = relations.Except(tags).ToArray();
      tags.AddRange(newers);
    }
  }
}
