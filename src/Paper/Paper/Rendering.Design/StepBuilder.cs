using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Paper.Media;
using Paper.Media.Design;

namespace Paper.Rendering.Design
{
  class StepBuilder<THost, TData> : IStepBuilder<THost, TData>
  {
    private const string HostKey = nameof(HostKey);
    private const string DataKey = nameof(DataKey);
    private const string EntityKey = nameof(EntityKey);

    private readonly List<Action<IPaperContext>> statements;

    public StepBuilder(
      Func<IPaperContext, THost> targetFactory,
      Func<IPaperContext, THost, TData> resultFactory,
      List<Action<IPaperContext>> outputStatements
      )
    {
      this.statements = outputStatements;
      this.statements.Add(ctx =>
      {
        var target = targetFactory.Invoke(ctx);
        ctx.Cache[HostKey] = target;

        var result = resultFactory.Invoke(ctx, target);
        ctx.Cache[DataKey] = result;
      });
    }

    public StepBuilder(
      Func<IPaperContext, THost> targetFactory,
      Func<IPaperContext, THost, FormData, TData> resultFactory,
      List<Action<IPaperContext>> outputStatements
      )
    {
      this.statements = outputStatements;
      this.statements.Add(ctx =>
      {
        var formData = ctx.ParseFormData();

        var target = targetFactory.Invoke(ctx);
        ctx.Cache[HostKey] = target;

        var result = resultFactory.Invoke(ctx, target, formData);
        ctx.Cache[DataKey] = result;
      });
    }

    public IRecordObjectGetter<TRecord> PopulateRecord<TRecord>(Func<IPaperContext, THost, TData, TRecord> populator)
    {
      var recordKey = $"Record-{Guid.NewGuid():B}";
      var nodeKey = $"Node-{recordKey}";
      this.statements.Add(ctx =>
      {
        var host = (THost)ctx.Cache[HostKey];
        var data = (TData)ctx.Cache[DataKey];
        var record = populator.Invoke(ctx, host, data);
        var node = new Node<TRecord> { Record = record };
        ctx.Cache[recordKey] = record;
        ctx.Cache[nodeKey] = node;
      });
      return new RecordGetter<TRecord>(recordKey, nodeKey);
    }

    public IRecordCollectionGetter<TRecord> PopulateRecords<TRecord>(Func<IPaperContext, THost, TData, ICollection<TRecord>> populator)
    {
      var recordsKey = $"Records-{Guid.NewGuid():B}";
      var nodesKey = $"Nodes-{recordsKey}";
      this.statements.Add(ctx =>
      {
        var host = (THost)ctx.Cache[HostKey];
        var data = (TData)ctx.Cache[DataKey];
        var records = populator.Invoke(ctx, host, data);
        var nodes = records.Select(x => new Node<TRecord> { Record = x }).ToArray();
        ctx.Cache[recordsKey] = records;
        ctx.Cache[nodesKey] = nodes;
      });
      return new RecordCollectionGetter<TRecord>(recordsKey, nodesKey);
    }

    public IRecordObjectGetter<TRecord> PopulateRecord<TBase, TRecord>(IRecordObjectGetter<TBase> @base, Func<IPaperContext, THost, TData, TBase, TRecord> populator) 
    {
      var recordKey = $"Record-{Guid.NewGuid():B}";
      var nodeKey = $"Node-{recordKey}";
      this.statements.Add(ctx =>
      {
        var parent = @base.GetNode(ctx);
        var host = (THost)ctx.Cache[HostKey];
        var data = (TData)ctx.Cache[DataKey];
        var record = populator.Invoke(ctx, host, data, parent.Record);
        var node = new Node<TRecord> { Parent = parent, Record = record };
        ctx.Cache[recordKey] = record;
        ctx.Cache[nodeKey] = node;
      });
      return new RecordGetter<TRecord>(recordKey, nodeKey);
    }

    public IRecordCollectionGetter<TRecord> PopulateRecords<TBase, TRecord>(IRecordObjectGetter<TBase> @base, Func<IPaperContext, THost, TData, TBase, ICollection<TRecord>> populator)
    {
      var recordsKey = $"Records-{Guid.NewGuid():B}";
      var nodesKey = $"Nodes-{recordsKey}";
      this.statements.Add(ctx =>
      {
        var parent = @base.GetNode(ctx);
        var host = (THost)ctx.Cache[HostKey];
        var data = (TData)ctx.Cache[DataKey];
        var records = populator.Invoke(ctx, host, data, parent.Record);
        var nodes = records.Select(x => new Node<TRecord> { Parent = parent, Record = x }).ToArray();
        ctx.Cache[recordsKey] = records;
        ctx.Cache[nodesKey] = nodes;
      });
      return new RecordCollectionGetter<TRecord>(recordsKey, nodesKey);
    }

    public IRecordObjectGetter<TRecord> PopulateRecord<TBase, TRecord>(IRecordCollectionGetter<TBase> @base, Func<IPaperContext, THost, TData, TBase, TRecord> populator)
    {
      var recordKey = $"Record-{Guid.NewGuid():B}";
      var nodeKey = $"Node-{recordKey}";
      this.statements.Add(ctx =>
      {
        var host = (THost)ctx.Cache[HostKey];
        var data = (TData)ctx.Cache[DataKey];

        var recordArray = new List<TRecord>();
        var nodeArray = new List<INode<TRecord>>();

        var items = @base.GetNodes(ctx);
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

    public IRecordCollectionGetter<TRecord> PopulateRecords<TBase, TRecord>(IRecordCollectionGetter<TBase> @base, Func<IPaperContext, THost, TData, TBase, ICollection<TRecord>> populator)
    {
      var recordKey = $"Record-{Guid.NewGuid():B}";
      var nodeKey = $"Node-{recordKey}";
      this.statements.Add(ctx =>
      {
        var host = (THost)ctx.Cache[HostKey];
        var data = (TData)ctx.Cache[DataKey];

        var recordArray = new List<ICollection<TRecord>>();
        var nodeArray = new List<ICollection<INode<TRecord>>>();

        var items = @base.GetNodes(ctx);
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

    public void CreateEntity<TRecord>(Func<IPaperContext, THost, TData, Entity> entityFactory)
    {
      this.statements.Add(ctx =>
      {
        var host = (THost)ctx.Cache[HostKey];
        var data = (TData)ctx.Cache[DataKey];
        var entity = entityFactory.Invoke(ctx, host, data);
        ctx.Cache[EntityKey] = entity;
      });
    }

    public void CreateEntity<TRecord>(IRecordObjectGetter<TRecord> getter, Func<IPaperContext, THost, TData, TRecord, Entity> entityFactory)
    {
      this.statements.Add(ctx =>
      {
        var host = (THost)ctx.Cache[HostKey];
        var data = (TData)ctx.Cache[DataKey];
        var node = getter.GetNode(ctx);

        node.Entity = entityFactory.Invoke(ctx, host, data, node.Record);

        if (node.Parent?.Entity is Entity parent)
          parent.WithEntities().Add(node.Entity);
        else
          ctx.Cache[EntityKey] = node.Entity;
      });
    }

    public void CreateEntity<TRecord>(IRecordCollectionGetter<TRecord> getter, Func<IPaperContext, THost, TData, ICollection<TRecord>, TRecord, Entity> entityFactory)
    {
      this.statements.Add(ctx =>
      {
        var host = (THost)ctx.Cache[HostKey];
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
          parent.WithEntities().Add(node.Entity);
        }
      });
    }

    public void FormatEntity<TRecord>(Action<IPaperContext, THost, TData, Entity> formatter)
    {
      this.statements.Add(ctx =>
      {
        var host = (THost)ctx.Cache[HostKey];
        var data = (TData)ctx.Cache[DataKey];

        var entity = (Entity)ctx.Cache[EntityKey];
        if (entity == null)
        {
          ctx.Cache[EntityKey] = CreateDefaultEntity(@object: null);
        }

        formatter.Invoke(ctx, host, data, entity);
      });
    }

    public void FormatEntity<TRecord>(IRecordObjectGetter<TRecord> getter, Action<IPaperContext, THost, TData, TRecord, Entity> formatter)
    {
      this.statements.Add(ctx =>
      {
        var host = (THost)ctx.Cache[HostKey];
        var data = (TData)ctx.Cache[DataKey];
        var node = getter.GetNode(ctx);

        if (node.Entity == null)
        {
          node.Entity = CreateDefaultEntity(node.Record);

          var parent = node.Parent?.Entity ?? (Entity)ctx.Cache[EntityKey];
          parent.WithEntities().Add(node.Entity);
        }

        formatter.Invoke(ctx, host, data, node.Record, node.Entity);
      });
    }

    public void FormatEntity<TRecord>(IRecordCollectionGetter<TRecord> getter, Action<IPaperContext, THost, TData, ICollection<TRecord>, TRecord, Entity> formatter)
    {
      this.statements.Add(ctx =>
      {
        var host = (THost)ctx.Cache[HostKey];
        var data = (TData)ctx.Cache[DataKey];
        var nodes = getter.GetNodes(ctx);
        var records = getter.GetRecords(ctx);

        foreach (var node in nodes)
        {
          if (node.Entity == null)
          {
            node.Entity = CreateDefaultEntity(node.Record);

            var parent = node.Parent?.Entity ?? (Entity)ctx.Cache[EntityKey];
            parent.WithEntities().Add(node.Entity);
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
