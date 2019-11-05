using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Toolset.Collections;

namespace Innkeeper.Rest
{
  internal class UrlPattern
  {
    public UrlPattern(string pattern)
    {
      ParsePattern(pattern);
    }

    public string Template { get; private set; }

    public bool Expanded => PathArgs?.LastOrDefault()?.Expanded ?? false;

    public PathArg[] PathArgs { get; private set; }

    public QueryArg[] QueryArgs { get; private set; }

    public override string ToString() => Template;

    private void ParsePattern(string template)
    {
      if (!template.StartsWith("/"))
        template = "/" + template;

      var tokens = template.Split('?');
      var path = tokens.First();
      var args = string.Join("?", tokens.Skip(1));

      var pathArgs = path.Split('/').Select((term, index) =>
      {
        if (!term.StartsWith("{"))
          return null;

        return new PathArg
        {
          SourceIndex = index,
          Target = Regex.Replace(term, "[{}*]", ""),
          Expanded = term.Contains("*")
        };
      }).NotNull().ToArray();

      var queryArgs = (
        from pair in args.Split('&')
        let parts = pair.Split('=')
        where parts.Length == 2
        where parts[1].StartsWith("{")
        select new QueryArg
        {
          Source = parts[0],
          Target = Regex.Replace(parts[1], "[{}?]", ""),
          IsRequired = !parts[1].Contains("?")
        }
      ).ToArray();

      this.Template = template;
      this.PathArgs = pathArgs.Any() ? pathArgs : null;
      this.QueryArgs = queryArgs.Any() ? queryArgs : null;
    }

    public class PathArg
    {
      public int SourceIndex { get; set; }

      public string Target { get; set; }

      public bool Expanded { get; set; }
    }

    public class QueryArg
    {
      public string Source { get; set; }

      public string Target { get; set; }

      public bool IsRequired { get; set; }
    }
  }
}
