using System;
using System.Linq;

namespace Paper.Media
{
  public class Rel : INode
  {
    public static readonly Rel Self = new Rel("self");

    public Rel(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
        throw new NullReferenceException("Um nome de relacionamento não deve ser nulo ou vazio.");

      this.Name = name.Trim();
    }

    public object Tag { get; set; }

    public string Name { get; }

    public bool IsMetaTag => char.IsUpper(Name.First());

    public override string ToString()
      => Name;

    public static implicit operator string(Rel rel)
      => rel.Name;

    public static implicit operator Rel(string rel)
      => rel == null ? null : new Rel(rel);
  }
}
