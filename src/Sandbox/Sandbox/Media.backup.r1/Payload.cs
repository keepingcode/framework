using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolset.Collections;
using Toolset.Sequel;

namespace Paper.Media
{
  public class Payload : IMediaObject, IValueCollection, IFormData, ISentData
  {
    public Record Error { get; set; }

    public Record Form { get; set; }

    public Record[] Records { get; set; }

    Payload IMediaObject.ExtractPayload() => this;

    #region Implementação de IValueCollection

    private IEnumerable<IValue> Enumerate()
    {
      if (Error != null) yield return Error;
      if (Form != null) yield return Form;
      if (Records != null) foreach (var record in Records) yield return record;
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

    #region Clonagem

    public Payload Clone()
    {
      return new Payload
      {
        Error = Error?.Clone(),
        Form = Form?.Clone(),
        Records = Records?.Select(x => x.Clone()).ToArray()
      };
    }

    IValue IValue.Clone() => Clone();

    object ICloneable.Clone() => Clone();


    #endregion
  }
}
