using System;
using System.Reflection;

namespace Innkeeper.Rest
{
  internal class RestPath
  {
    public Type PipelineType { get; set; }

    public MethodInfo Method { get; set; }

    public UrlPattern Pattern { get; set; }

    public string Verb { get; set; }

    public override string ToString()
    {
      return $"{Verb} {Method.Name}() {Pattern}";
    }
  }
}