using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Paper.Media3;
using Paper.Media3.Serialization;
using Toolset;
using Toolset.Collections;

namespace Sandbox
{
  class TBusuario
  {
    public int? DFid_usuario { get; set; }
    public string DFnome_usuario { get; set; }
    public TBempresa DFempresa { get; set; }
    public List<TBnivel> DFniveis { get; set; }
    public string[] DFapelidos { get; set; }
  }

  class TBempresa
  {
    public int? DFid_empresa { get; set; }
    public string DFdescricao { get; set; }
  }

  class TBnivel
  {
    public int? DFid_nivel { get; set; }
    public string DFdescricao { get; set; }
  }

  public static class Program
  {
    public static void Main(string[] args)
    {
      try
      {
        var fulano = new TBusuario
        {
          DFid_usuario = 12,
          DFnome_usuario = "Fulano",
          DFempresa = new TBempresa
          {
            DFid_empresa = 43,
            DFdescricao = "Peanuts Cia"
          },
          DFniveis = new List<TBnivel>
          {
            new TBnivel
            {
              DFid_nivel = 201,
              DFdescricao = "Peasant"
            },
            new TBnivel
            {
              DFid_nivel = 346,
              DFdescricao = "Liutenant"
            }
          },
          DFapelidos = new[] { "Santa", "Claus" }
        };

        var entity = Entity.Create(fulano);
        entity.Properties["Status"] = "Active";
        entity.Properties["DFid_usuario"] = "123";

        var niveis = (IPropertyList)entity.Properties["DFniveis"];
        niveis.Add(new TBnivel()
        {
          DFid_nivel = 281,
          DFdescricao = "One of That"
        });

        var apelidos = (IPropertyList)entity.Properties["DFapelidos"];
        apelidos[1] = "Papa";





        entity.Properties["list"] = new List<object>();
        var items = entity.Properties["list"] as IPropertyList;
        items.Add("teste");
        items.Add(new
        {
          Id = 10,
          Name = "Talz"
        });
        items.Add(new HashMap
        {
          { "Bem", (CaseVariantString) "Tananana" },
          { "Preço", new object[]
            {
              new Var(new Range(4, 12.801)),
              new Var(33),
              new Var(new[] { "a", "b" })
            }
          }
        });



        var stack = new Stack<Tuple<string, object>>();
        stack.Push(new Tuple<string, object>("Fulano", entity.Properties));
        while (stack.Count > 0)
        {
          var entry = stack.Pop();
          var path = entry.Item1;
          var item = entry.Item2;

          if (item is IPropertyMap map)
          {
            foreach (var property in map.PropertyNames)
            {
              var key = $"{path}.{property}";
              var value = map[property];
              stack.Push(new Tuple<string, object>(key, value));
            }
          }
          else if (item is IPropertyList list)
          {
            for (int i = 0; i < list.Count; i++)
            {
              var key = $"{path}.{i}";
              var value = list[i];
              stack.Push(new Tuple<string, object>(key, value));
            }
          }
          else
          {
            Debug.WriteLine($"{path,-30}: {item}");
          }
        }
      }
      catch (Exception ex)
      {
        ex.Trace();
      }
    }
  }
}