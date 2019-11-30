using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolset.Collections;

namespace Paper.Media
{
  public class Payload : IMediaObject, IValueCollection, IFormData, ISentData
  {
    public PropertyCollection Error { get; set; }

    public PropertyCollection Form { get; set; }

    public PropertyCollection Records { get; set; }

    Payload IMediaObject.ToPayload() => this;

    #region Implementação de IValueCollection

    private IEnumerable<IValue> Enumerate()
    {
      if (Error != null) yield return Error;
      if (Form != null) yield return Form;
      if (Records != null) yield return Records;
    }

    IEnumerator<IValue> IEnumerable<IValue>.GetEnumerator()
    {
      return Enumerate().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return Enumerate().GetEnumerator();
    }

    object IValue.Value => Enumerate().ToArray();

    int IValueCollection.Count => Enumerate().Count();

    IValue IValueCollection.this[int index] => Enumerate().ElementAt(index);

    #endregion
  }
}
