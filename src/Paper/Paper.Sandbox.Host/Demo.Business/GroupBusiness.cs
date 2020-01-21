using Paper.Design;
using Paper.Sandbox.Host.Demo.Domain;
using Paper.Sandbox.Host.Demo.Filters;
using Paper.Sandbox.Host.Demo.Forms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Toolset;
using Toolset.Reflection;

namespace Paper.Sandbox.Host.Demo.Business
{
  public class GroupBusiness
  {
    public Ret<ICollection<Group>> Find(Page page, Sort sort, GroupFilter filter)
    {
      return Db.Groups.SortBy(sort).PaginateBy(page).FilterBy(filter).ToArray();
    }

    public Ret<ICollection<Group>> Create(Bulk<GroupForm.Create> forms)
    {
      var result = new List<Group>();

      // Validando
      foreach (var form in forms)
      {
        var name = form.Name?.Trim();

        if (string.IsNullOrWhiteSpace(name))
          return Ret.Fail(HttpStatusCode.BadRequest, $"O nome de grupo deve ser informado.");

        if (Db.Groups.Any(x => x.Name == name))
          return Ret.Fail(HttpStatusCode.Conflict, $"Já existe um grupo de usuário com este nome.");
      }

      // Editando
      foreach (var form in forms)
      {
        var group = new Group
        {
          Id = Db.GenerateId(),
          Name = form.Name.Trim()
        };
        Db.Groups.Add(group);
        result.Add(group);
      }

      return result;
    }

    public Ret<ICollection<Group>> Edit(Bulk<GroupForm.BulkEdit, Group> edits)
    {
      var result = new List<Group>();

      // Validando
      foreach (var edit in edits)
      {
        edit.Record = Db.Groups.FirstOrDefault(x => x.Id == edit.Record.Id);
        if (edit.Record == null)
          return Ret.Fail(HttpStatusCode.NotFound, $"O grupo de usuário não existe.");

        var form = edit.Form;
        var group = edit.Record;

        var name = form.Name ?? group.Name;

        if (string.IsNullOrWhiteSpace(name))
          return Ret.Fail(HttpStatusCode.BadRequest, $"O nome do grupo de usuário deve ser informado.");
      }

      // Editando
      foreach (var edit in edits)
      {
        var form = edit.Form;
        var group = edit.Record;
        group.Name = (form.Name ?? group.Name)?.Trim();
        result.Add(group);
      }

      return result;
    }

    public Ret Delete(Bulk<Group> groups)
    {
      foreach (var group in groups)
      {
        Db.Groups.RemoveAll(x => x.Id == group.Id);
      }
      return Ret.OK();
    }
  }
}
