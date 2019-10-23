using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Toolset.Reflection;

namespace Paper.Design
{
  public struct PaperId
  {
    private readonly string id;

    public string Module => id.Split('/').First();

    public string Schema => id.Split('/').Last();

    public PaperId(string paperId)
    {
      this.id = paperId;
    }

    public PaperId(string module, string schema)
    {
      this.id = $"{module}/{schema}";
    }

    public override string ToString()
    {
      return id;
    }

    public static PaperId Identify(Type paperType)
    {
      var attr = paperType._GetAttribute<PaperAttribute>();
      var module = attr?.Module;
      var schema = attr?.Schema;

      if (module == null)
      {
        module = paperType.Namespace;
        schema = paperType.Name;
      }

      return new PaperId(module, schema);
    }

    public override int GetHashCode()
    {
      return id.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      var otherId = (obj is PaperId paperId) ? paperId.id : obj as string;
      return id.Equals(otherId);
    }

    public static implicit operator PaperId(string paperId)
    {
      return new PaperId(paperId);
    }

    public static implicit operator string(PaperId paperId)
    {
      return paperId.ToString();
    }
  }
}
