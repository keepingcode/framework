using Paper.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Rendering.Design
{
  public interface IStatementDesigner<TPaperInstance, TData>
  {
    IRecordObjectGetter<TRecord> PopulateRecord<TRecord>(Func<IPaperContext, TPaperInstance, TData, TRecord> populator);
    IRecordCollectionGetter<TRecord> PopulateRecords<TRecord>(Func<IPaperContext, TPaperInstance, TData, ICollection<TRecord>> populator);

    IRecordObjectGetter<TRecord> PopulateRecord<TRef, TRecord>(IRecordObjectGetter<TRef> @ref, Func<IPaperContext, TPaperInstance, TData, TRef, TRecord> populator);
    IRecordCollectionGetter<TRecord> PopulateRecords<TRef, TRecord>(IRecordObjectGetter<TRef> @ref, Func<IPaperContext, TPaperInstance, TData, TRef, ICollection<TRecord>> populator);

    IRecordObjectGetter<TRecord> PopulateRecord<TRef, TRecord>(IRecordCollectionGetter<TRef> @ref, Func<IPaperContext, TPaperInstance, TData, TRef, TRecord> populator);
    IRecordCollectionGetter<TRecord> PopulateRecords<TRef, TRecord>(IRecordCollectionGetter<TRef> @ref, Func<IPaperContext, TPaperInstance, TData, TRef, ICollection<TRecord>> populator);

    void CreateEntity<TRecord>(Func<IPaperContext, TPaperInstance, TData, Entity> formatter);
    void CreateEntity<TRecord>(IRecordObjectGetter<TRecord> getter, Func<IPaperContext, TPaperInstance, TData, TRecord, Entity> formatter);
    void CreateEntity<TRecord>(IRecordCollectionGetter<TRecord> getter, Func<IPaperContext, TPaperInstance, TData, ICollection<TRecord>, TRecord, Entity> formatter);

    void FormatEntity<TRecord>(Action<IPaperContext, TPaperInstance, TData, Entity> formatter);
    void FormatEntity<TRecord>(IRecordObjectGetter<TRecord> getter, Action<IPaperContext, TPaperInstance, TData, TRecord, Entity> formatter);
    void FormatEntity<TRecord>(IRecordCollectionGetter<TRecord> getter, Action<IPaperContext, TPaperInstance, TData, ICollection<TRecord>, TRecord, Entity> formatter);
  }
}
