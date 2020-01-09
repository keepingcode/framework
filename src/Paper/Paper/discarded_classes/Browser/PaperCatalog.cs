using Paper.Design;
using System;
using System.Collections.Generic;
using System.Text;
using Toolset;
using Toolset.Collections;

namespace Paper.Browser
{
  internal class PaperCatalog : Map<PaperId, Type>, IPaperCatalog
  {
    public Type GetPaperType(PaperId paperId)
    {
      return this[paperId];
    }

    public void Fill()
    {
      try
      {
        var paperTypes = ExposedTypes.GetTypes<IPaper>();
        foreach (var paperType in paperTypes)
        {
          try
          {
            var paperId = PaperId.Identify(paperType);
            Add(paperId, paperType);
          }
          catch (Exception ex)
          {
            ex.Trace($"Falha importando o esquema de Paper: {paperType.FullName}");
          }
        }
      }
      catch (Exception ex)
      {
        ex.Trace("Não foi possível importar os esquemas de Papers da pasta do aplicativo.");
      }
    }
  }
}
