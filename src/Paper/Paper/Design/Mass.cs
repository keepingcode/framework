using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Paper.Design
{
  public static class Mass
  {
    public static Mass<TForm, TRecord> Create<TForm, TRecord>(TForm form, params TRecord[] records)
    {
      var mass = new Mass<TForm, TRecord>();
      mass.Form = form;
      mass.AddMany(records);
      return mass;
    }

    public static Mass<TForm, TRecord> Create<TForm, TRecord>(TForm form, ICollection<TRecord> records)
    {
      var mass = new Mass<TForm, TRecord>();
      mass.Form = form;
      mass.AddMany(records);
      return mass;
    }

    public static Mass<TForm, TRecord> Create<TForm, TRecord>(TForm form, IEnumerable<TRecord> records)
    {
      var mass = new Mass<TForm, TRecord>();
      mass.Form = form;
      mass.AddMany(records);
      return mass;
    }
  }
}
