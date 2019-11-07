using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Toolset;
using Toolset.Collections;

namespace Innkeeper.Host
{
  public class QueryArgs : IArgs
  {
    private readonly HashMap<Var> args = new HashMap<Var>();

    public QueryArgs(string uri)
    {
      var queryString = new UriString(uri);
      foreach (var name in queryString.GetArgNames())
      {
        var value = queryString.GetArg(name);
        args[name] = value is Var var ? var : new Var(value);
      }
    }

    public int Count => args.Count;

    public ICollection<string> Keys => args.Keys;

    public Var this[string key]
    {
      get => args[key];
      set => args[key] = value;
    }

    public IEnumerator<KeyValuePair<string, Var>> GetEnumerator() => args.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => args.GetEnumerator();
  }
}
