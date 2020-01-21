using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Innkeeper.Rest;
using Paper.Media;
using Paper.Media.Design;
using Toolset;

namespace Paper.Rendering.Design
{
  internal class StatementDesigner<TPaperInstance, TData> : IStatementDesigner<TPaperInstance, TData>
  {
    private const string PaperInstanceKey = nameof(PaperInstanceKey);
    private const string DataKey = nameof(DataKey);
    private const string EntityKey = nameof(EntityKey);

    private readonly List<Action<IPaperContext>> statements = new List<Action<IPaperContext>>();

    public StatementDesigner(
      Func<IPaperContext, TPaperInstance> paperInstanceFactory,
      Func<IPaperContext, TPaperInstance, IEntity, TData> dataFactory)
    {
      SelectData(paperInstanceFactory, dataFactory);
    }

    public Func<IPaperContext, IEntity> DesignStatement()
    {
      return ctx =>
      {
        try
        {
          foreach (var statement in statements)
          {
            statement.Invoke(ctx);
          }

          var result = ctx.Cache[EntityKey] as IEntity;
          if (result == null)
          {
            result = Entity.Create(HttpStatusCode.NotFound);
          }

          return result;
        }
        catch (Exception ex)
        {
          var result = Entity.Create(ex);
          ctx.Cache[EntityKey] = result;
          throw;
        }
      };
    }

    private void SelectData(
      Func<IPaperContext, TPaperInstance> hostFactory,
      Func<IPaperContext, TPaperInstance, IEntity, TData> dataFactory)
    {
      this.statements.Add(ctx =>
      {
        IEntity formData = null;

        if (ctx.Verb == VerbNames.Get)
        {
          var dataCollection = ctx.IncomingData?.ReadData();
          formData = dataCollection?.FirstOrDefault();
        }

        TPaperInstance host = hostFactory.Invoke(ctx);
        ctx.Cache[PaperInstanceKey] = host;

        TData data = dataFactory.Invoke(ctx, host, formData);
        ctx.Cache[DataKey] = data;
      });
    }

    public IRecordObjectGetter<TRecord> PopulateRecord<TRecord>(Func<IPaperContext, TPaperInstance, TData, TRecord> populator)
    {
      var recordKey = $"Record-{Guid.NewGuid():B}";
      var nodeKey = $"Node-{recordKey}";
      this.statements.Add(ctx =>
      {
        var host = (TPaperInstance)ctx.Cache[PaperInstanceKey];
        var data = (TData)ctx.Cache[DataKey];
        var record = populator.Invoke(ctx, host, data);
        var node = new Node<TRecord> { Record = record };
        ctx.Cache[recordKey] = record;
        ctx.Cache[nodeKey] = node;
      });
      return new RecordGetter<TRecord>(recordKey, nodeKey);
    }

    public IRecordCollectionGetter<TRecord> PopulateRecords<TRecord>(Func<IPaperContext, TPaperInstance, TData, ICollection<TRecord>> populator)
    {
      var recordsKey = $"Records-{Guid.NewGuid():B}";
      var nodesKey = $"Nodes-{recordsKey}";
      this.statements.Add(ctx =>
      {
        var host = (TPaperInstance)ctx.Cache[PaperInstanceKey];
        var data = (TData)ctx.Cache[DataKey];
        var records = populator.Invoke(ctx, host, data);
        var nodes = records.Select(x => new Node<TRecord> { Record = x }).ToArray();
        ctx.Cache[recordsKey] = records;
        ctx.Cache[nodesKey] = nodes;
      });
      return new RecordCollectionGetter<TRecord>(recordsKey, nodesKey);
    }

    public IRecordObjectGetter<TRecord> PopulateRecord<TRef, TRecord>(IRecordObjectGetter<TRef> @ref, Func<IPaperContext, TPaperInstance, TData, TRef, TRecord> populator) 
    {
      var recordKey = $"Record-{Guid.NewGuid():B}";
      var nodeKey = $"Node-{recordKey}";
      this.statements.Add(ctx =>
      {
        var parent = @ref.GetNode(ctx);
        var host = (TPaperInstance)ctx.Cache[PaperInstanceKey];
        var data = (TData)ctx.Cache[DataKey];
        var record = populator.Invoke(ctx, host, data, parent.Record);
        var node = new Node<TRecord> { Parent = parent, Record = record };
        ctx.Cache[recordKey] = record;
        ctx.Cache[nodeKey] = node;
      });
      return new RecordGetter<TRecord>(recordKey, nodeKey);
    }

    public IRecordCollectionGetter<TRecord> PopulateRecords<TRef, TRecord>(IRecordObjectGetter<TRef> @ref, Func<IPaperContext, TPaperInstance, TData, TRef, ICollection<TRecord>> populator)
    {
      var recordsKey = $"Records-{Guid.NewGuid():B}";
      var nodesKey = $"Nodes-{recordsKey}";
      this.statements.Add(ctx =>
      {
        var parent = @ref.GetNode(ctx);
        var host = (TPaperInstance)ctx.Cache[PaperInstanceKey];
        var data = (TData)ctx.Cache[DataKey];
        var records = populator.Invoke(ctx, host, data, parent.Record);
        var nodes = records.Select(x => new Node<TRecord> { Parent = parent, Record = x }).ToArray();
        ctx.Cache[recordsKey] = records;
        ctx.Cache[nodesKey] = nodes;
      });
      return new RecordCollectionGetter<TRecord>(recordsKey, nodesKey);
    }

    public IRecordObjectGetter<TRecord> PopulateRecord<TRef, TRecord>(IRecordCollectionGetter<TRef> @ref, Func<IPaperContext, TPaperInstance, TData, TRef, TRecord> populator)
    {
      var recordKey = $"Record-{Guid.NewGuid():B}";
      var nodeKey = $"Node-{recordKey}";
      this.statements.Add(ctx =>
      {
        var host = (TPaperInstance)ctx.Cache[PaperInstanceKey];
        var data = (TData)ctx.Cache[DataKey];

        var recordArray = new List<TRecord>();
        var nodeArray = new List<INode<TRecord>>();

        var items = @ref.GetNodes(ctx);
        foreach (var parent in items)
        {
          var record = populator.Invoke(ctx, host, data, parent.Record);
          var node = new Node<TRecord> { Parent = parent, Record = record };

          recordArray.Add(record);
          nodeArray.Add(node);
        }

        ctx.Cache[recordKey] = recordArray;
        ctx.Cache[nodeKey] = nodeArray;
      });
      return new RecordGetter<TRecord>(recordKey, nodeKey);
    }

    public IRecordCollectionGetter<TRecord> PopulateRecords<TRef, TRecord>(IRecordCollectionGetter<TRef> @ref, Func<IPaperContext, TPaperInstance, TData, TRef, ICollection<TRecord>> populator)
    {
      var recordKey = $"Record-{Guid.NewGuid():B}";
      var nodeKey = $"Node-{recordKey}";
      this.statements.Add(ctx =>
      {
        var host = (TPaperInstance)ctx.Cache[PaperInstanceKey];
        var data = (TData)ctx.Cache[DataKey];

        var recordArray = new List<ICollection<TRecord>>();
        var nodeArray = new List<ICollection<INode<TRecord>>>();

        var items = @ref.GetNodes(ctx);
        foreach (var parent in items)
        {
          var records = populator.Invoke(ctx, host, data, parent.Record);
          var nodes = records.Select(x => new Node<TRecord> { Parent = parent, Record = x }).ToArray();

          recordArray.Add(records);
          nodeArray.Add(nodes);
        }

        ctx.Cache[recordKey] = recordArray;
        ctx.Cache[nodeKey] = nodeArray;
      });
      return new RecordCollectionGetter<TRecord>(recordKey, nodeKey);
    }

    public void CreateEntity<TRecord>(Func<IPaperContext, TPaperInstance, TData, Entity> entityFactory)
    {
      this.statements.Add(ctx =>
      {
        if (ctx.OutgoingData.IsPayload)
          return;

        var host = (TPaperInstance)ctx.Cache[PaperInstanceKey];
        var data = (TData)ctx.Cache[DataKey];
        var entity = entityFactory.Invoke(ctx, host, data);
        ctx.Cache[EntityKey] = entity;
      });
    }

    public void CreateEntity<TRecord>(IRecordObjectGetter<TRecord> getter, Func<IPaperContext, TPaperInstance, TData, TRecord, Entity> entityFactory)
    {
      this.statements.Add(ctx =>
      {
        if (ctx.OutgoingData.IsPayload)
          return;

        var host = (TPaperInstance)ctx.Cache[PaperInstanceKey];
        var data = (TData)ctx.Cache[DataKey];
        var node = getter.GetNode(ctx);

        node.Entity = entityFactory.Invoke(ctx, host, data, node.Record);

        if (node.Parent?.Entity is Entity parent)
        {
          parent.Add(node.Entity);
        }
        else
        {
          ctx.Cache[EntityKey] = node.Entity;
        }
      });
    }

    public void CreateEntity<TRecord>(IRecordCollectionGetter<TRecord> getter, Func<IPaperContext, TPaperInstance, TData, ICollection<TRecord>, TRecord, Entity> entityFactory)
    {
      this.statements.Add(ctx =>
      {
        if (ctx.OutgoingData.IsPayload)
          return;

        var host = (TPaperInstance)ctx.Cache[PaperInstanceKey];
        var data = (TData)ctx.Cache[DataKey];
        var nodes = getter.GetNodes(ctx);
        var records = getter.GetRecords(ctx);

        if (!ctx.Cache.ContainsKey(EntityKey))
        {
          ctx.Cache[EntityKey] = CreateDefaultEntity(@object: null);
        }

        foreach (var node in nodes)
        {
          node.Entity = entityFactory.Invoke(ctx, host, data, records, node.Record);

          var parent = node.Parent?.Entity ?? (Entity)ctx.Cache[EntityKey];
          parent.Add(node.Entity);
        }
      });
    }

    public void FormatEntity<TRecord>(Action<IPaperContext, TPaperInstance, TData, Entity> formatter)
    {
      this.statements.Add(ctx =>
      {
        if (ctx.OutgoingData.IsPayload)
          return;

        var host = (TPaperInstance)ctx.Cache[PaperInstanceKey];
        var data = (TData)ctx.Cache[DataKey];

        var entity = (Entity)ctx.Cache[EntityKey];
        if (entity == null)
        {
          ctx.Cache[EntityKey] = CreateDefaultEntity(@object: null);
        }

        formatter.Invoke(ctx, host, data, entity);
      });
    }

    public void FormatEntity<TRecord>(IRecordObjectGetter<TRecord> getter, Action<IPaperContext, TPaperInstance, TData, TRecord, Entity> formatter)
    {
      this.statements.Add(ctx =>
      {
        if (ctx.OutgoingData.IsPayload)
          return;

        var host = (TPaperInstance)ctx.Cache[PaperInstanceKey];
        var data = (TData)ctx.Cache[DataKey];
        var node = getter.GetNode(ctx);

        if (node.Entity == null)
        {
          node.Entity = CreateDefaultEntity(node.Record);

          var parent = node.Parent?.Entity ?? (Entity)ctx.Cache[EntityKey];
          parent.Add(node.Entity);
        }

        formatter.Invoke(ctx, host, data, node.Record, node.Entity);
      });
    }

    public void FormatEntity<TRecord>(IRecordCollectionGetter<TRecord> getter, Action<IPaperContext, TPaperInstance, TData, ICollection<TRecord>, TRecord, Entity> formatter)
    {
      this.statements.Add(ctx =>
      {
        if (ctx.OutgoingData.IsPayload)
          return;

        var host = (TPaperInstance)ctx.Cache[PaperInstanceKey];
        var data = (TData)ctx.Cache[DataKey];
        var nodes = getter.GetNodes(ctx);
        var records = getter.GetRecords(ctx);

        foreach (var node in nodes)
        {
          if (node.Entity == null)
          {
            node.Entity = CreateDefaultEntity(node.Record);

            var parent = node.Parent?.Entity ?? (Entity)ctx.Cache[EntityKey];
            parent.Add(node.Entity);
          }

          formatter.Invoke(ctx, host, data, records, node.Record, node.Entity);
        }
      });
    }

    private Entity CreateDefaultEntity(object @object)
    {
      var entity = new Entity();
      return entity;
    }
  }
}
