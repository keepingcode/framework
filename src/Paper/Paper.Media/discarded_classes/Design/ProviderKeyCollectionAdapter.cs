using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paper.Media.Design
{
  public class ProviderKeyCollectionAdapter : IEnumerable<CaseVariantString>
  {
    private Func<PropertyValueCollection> getter;

    internal ProviderKeyCollectionAdapter(Func<PropertyValueCollection> getter)
    {
      this.getter = getter;
    }

    public PropertyValueCollection Collection => getter.Invoke();

    public int Count => Collection.Count;

    public CaseVariantString this[int index]
    {
      get => (CaseVariantString)Collection[index];
      set => Collection[index] = value;
    }

    public void Add(CaseVariantString key)
    {
      Collection.Add(key);
    }

    public void AddAt(int index, CaseVariantString key)
    {
      Collection.AddAt(index, key);
    }

    public void AddMany(IEnumerable<CaseVariantString> keys)
    {
      Collection.AddMany(keys);
    }

    public void Remove(CaseVariantString key)
    {
      Collection.Remove(key);
    }

    public void RemoveAt(int index)
    {
      Collection.RemoveAt(index);
    }

    public void RemoveMany(IEnumerable<CaseVariantString> keys)
    {
      Collection.RemoveMany(keys);
    }

    public void RemoveRange(int index, int count)
    {
      Collection.RemoveRange(index, count);
    }

    public void RemoveWhen(Predicate<CaseVariantString> match)
    {
      Collection.RemoveWhen(key => match.Invoke((CaseVariantString)key));
    }

    public IEnumerator<CaseVariantString> GetEnumerator()
    {
      return Collection.Select(x => (CaseVariantString)x).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}