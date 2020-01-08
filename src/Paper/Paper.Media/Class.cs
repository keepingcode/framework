using System;
using System.Linq;

namespace Paper.Media
{
  public class Class : INode
  {
    public static readonly Class Record = new Class("record");
    public static readonly Class Resource = new Class("resource");
    public static readonly Class Action = new Class("action");
    public static readonly Class Field = new Class("field");

    public Class(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
        throw new NullReferenceException("Um nome de classe não deve ser nulo ou vazio.");

      this.Name = name.Trim();
    }

    public object Tag { get; set; }

    public string Name { get; }

    public bool IsMetaTag => char.IsUpper(Name.First());

    public override string ToString()
      => Name;

    public static implicit operator string(Class @class)
      => @class.Name;

    public static implicit operator Class(string @class)
      => @class == null ? null : new Class(@class);
  }
}
