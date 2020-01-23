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

    /// <summary>
    /// Por padrão, algumas propriedades são renderizadas no corpo da entidade e na sua
    /// coleção de propriedades.
    /// 
    /// Por exemplo:
    /// {
    ///   "title": TITLE,
    ///   "properties: {
    ///     "title": TITLE
    ///   }
    /// }
    /// 
    /// Para omitir a renderização de propriedades como estas na coleção de propriedades
    /// da entidade este campo "Hide" pode ser marcado como verdadeiro.
    /// 
    /// No exemplo acima, se "Hide" for verdadeiro, o resultado se torna:
    /// {
    ///   "title": TITLE
    /// }
    /// 
    /// Embora menos comum, o campo "Hide" pode ser usado para apenas omitir uma propriedade
    /// qualquer do renderizador.
    /// Se uma propriedade qualquer tem seu campo "Hide" como verdadeiro o renderizador
    /// simplismente a ignora.
    /// </summary>
    public bool Hidden { get; set; }

    public IValue Value { get; set; }

    public Property Clone()
      => new Property(Name,
           (Value is ICloneable cloneable) ? (IValue)cloneable.Clone() : Value
         );
    
    object ICloneable.Clone()
      => Clone();
  }
}
