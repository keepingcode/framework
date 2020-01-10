using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Design
{
  public static class Bulk
  {
    public static Bulk<TForm> Create<TForm>(params TForm[] forms)
    {
      var bulk = new Bulk<TForm>();
      bulk.AddMany(forms);
      return bulk;
    }

    public static Bulk<TForm, TRecord> Create<TForm, TRecord>(TForm form, TRecord record)
    {
      return new Bulk<TForm, TRecord>
      {
        new Bulk<TForm, TRecord>.Entry
        {
          Form = form,
          Record = record
        }
      };
    }
  }
}
