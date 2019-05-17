using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Toolset;
using Toolset.Collections;

namespace Paper.Media2
{
  public interface IMediaManager
  {
    T Create<T>()
      where T : class, IMediaObject, new();

    Uid Add<T>(T media)
      where T : class, IMediaObject;

    T Get<T>(Uid uid)
      where T : class, IMediaObject;

    IEnumerable<T> List<T>()
      where T : class, IMediaObject;

    IEnumerable<T> List<T>(Uid parent)
      where T : class, IMediaObject;

    void Bind(Uid parent, params Uid[] children);

    void Bind(Uid parent, IEnumerable<Uid> children);

    void Tag(Uid parent, params Rel[] tags);

    void Tag(Uid parent, IEnumerable<Rel> tags);

    void Tag(Uid parent, params string[] tags);

    void Tag(Uid parent, IEnumerable<string> tags);
  }
}
