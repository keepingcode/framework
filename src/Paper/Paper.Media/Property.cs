using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolset;
using Toolset.Collections;

namespace Paper.Media
{
  public class Property : INode, ICloneable
  {
    public Property(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
        throw new NullReferenceException("O nome de uma propriedade não deve ser nulo ou vazio.");

      this.Name = name.Trim().ChangeCase(TextCase.PascalCase);
    }

    public Property(string name, object value)
    {
      if (string.IsNullOrWhiteSpace(name))
        throw new NullReferenceException("O nome de uma propriedade não deve ser nulo ou vazio.");

      this.Name = name.Trim().ChangeCase(TextCase.PascalCase);
      this.Value = Media.Value.Create(value);
    }

    public object Tag { get; set; }

    public string Name { get; }

    public IValue Value { get; set; }

    public Property Clone()
      => new Property(Name,
           (Value is ICloneable cloneable) ? (IValue)cloneable.Clone() : Value
         );
    
    object ICloneable.Clone()
      => Clone();
  }
}
