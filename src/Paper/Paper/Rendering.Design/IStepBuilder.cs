using Paper.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Rendering.Design
{
  public interface IStepBuilder<THost, TData>
  {
    IRecordObjectGetter<TRecord> PopulateRecord<TRecord>(Func<IPaperContext, THost, TData, TRecord> populator);
    IRecordCollectionGetter<TRecord> PopulateRecords<TRecord>(Func<IPaperContext, THost, TData, ICollection<TRecord>> populator);

    IRecordObjectGetter<TRecord> PopulateRecord<TBase, TRecord>(IRecordObjectGetter<TBase> @base, Func<IPaperContext, THost, TData, TBase, TRecord> populator);
    IRecordCollectionGetter<TRecord> PopulateRecords<TBase, TRecord>(IRecordObjectGetter<TBase> @base, Func<IPaperContext, THost, TData, TBase, ICollection<TRecord>> populator);

    IRecordObjectGetter<TRecord> PopulateRecord<TBase, TRecord>(IRecordCollectionGetter<TBase> @base, Func<IPaperContext, THost, TData, TBase, TRecord> populator);
    IRecordCollectionGetter<TRecord> PopulateRecords<TBase, TRecord>(IRecordCollectionGetter<TBase> @base, Func<IPaperContext, THost, TData, TBase, ICollection<TRecord>> populator);

    void CreateEntity<TRecord>(Func<IPaperContext, THost, TData, Entity> formatter);
    void CreateEntity<TRecord>(IRecordObjectGetter<TRecord> getter, Func<IPaperContext, THost, TData, TRecord, Entity> formatter);
    void CreateEntity<TRecord>(IRecordCollectionGetter<TRecord> getter, Func<IPaperContext, THost, TData, ICollection<TRecord>, TRecord, Entity> formatter);

    void FormatEntity<TRecord>(Action<IPaperContext, THost, TData, Entity> formatter);
    void FormatEntity<TRecord>(IRecordObjectGetter<TRecord> getter, Action<IPaperContext, THost, TData, TRecord, Entity> formatter);
    void FormatEntity<TRecord>(IRecordCollectionGetter<TRecord> getter, Action<IPaperContext, THost, TData, ICollection<TRecord>, TRecord, Entity> formatter);
  }
}
