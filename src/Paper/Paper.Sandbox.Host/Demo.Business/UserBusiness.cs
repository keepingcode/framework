using Paper.Design;
using Paper.Sandbox.Host.Demo.Databases;
using Paper.Sandbox.Host.Demo.Domain;
using Paper.Sandbox.Host.Demo.Filters;
using Paper.Sandbox.Host.Demo.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Toolset;
using Toolset.Reflection;

namespace Paper.Sandbox.Host.Demo.Business
{
  public class UserBusiness
  {
    public Ret<ICollection<User>> Find(Page page, Sort sort, UserFilter filter)
    {
      return Db.Users.SortBy(sort).PaginateBy(page).FilterBy(filter).ToArray();
    }

    public Ret<ICollection<User>> Create(Bulk<UserForm.Create> forms)
    {
      var result = new List<User>();

      // Validando
      foreach (var form in forms)
      {
        if (Db.Users.Any(x => x.Login == form.Login))
          return Ret.Fail(HttpStatusCode.Conflict, $"Já existe uma conta de usuário para este login.");

        if (string.IsNullOrWhiteSpace(form.Login))
          return Ret.Fail(HttpStatusCode.BadRequest, $"O nome de login deve ser informado.");

        if (string.IsNullOrWhiteSpace(form.Name))
          return Ret.Fail(HttpStatusCode.BadRequest, $"O nome de usuário deve ser informado.");

        if (!Db.Groups.Any(x => x.Id == form.GroupId))
          return Ret.Fail(HttpStatusCode.BadRequest, $"O grupo de usuário não existe.");
      }

      // Editando
      foreach (var form in forms)
      {
        var user = new User
        {
          Id = Db.GenerateId(),
          Login = form.Login.Trim(),
          Name = form.Name.Trim(),
          Enabled = true,
          GroupId = form.GroupId
        };
        Db.Users.Add(user);
        result.Add(user);
      }

      return result;
    }

    public Ret<ICollection<User>> Edit(Bulk<UserForm.BulkEdit, User> edits)
    {
      var result = new List<User>();

      // Validando
      foreach (var edit in edits)
      {
        edit.Record = Db.Users.FirstOrDefault(x => x.Id == edit.Record.Id);
        if (edit.Record == null)
          return Ret.Fail(HttpStatusCode.NotFound, $"A conta de usuário não existe.");

        var form = edit.Form;
        var user = edit.Record;

        var name = form.Name ?? user.Name;
        var enabled = form.Enabled ?? user.Enabled;
        var groupId = form.GroupId ?? user.GroupId;

        if (string.IsNullOrWhiteSpace(name))
          return Ret.Fail(HttpStatusCode.BadRequest, $"O nome de usuário deve ser informado.");

        if (!Db.Groups.Any(x => x.Id == groupId))
          return Ret.Fail(HttpStatusCode.BadRequest, $"O grupo de usuário não existe.");
      }

      // Editando
      foreach (var edit in edits)
      {
        var form = edit.Form;
        var user = edit.Record;
        user.Name = (form.Name ?? user.Name)?.Trim();
        user.Enabled = form.Enabled ?? user.Enabled;
        user.GroupId = form.GroupId ?? user.GroupId;
        result.Add(user);
      }

      return result;
    }

    public Ret<ICollection<User>> Edit(Mass<UserForm.MassEdit, User> edits)
    {
      var result = new List<User>();

      var form = edits.Form;
      var users = (
        from user in edits
        select Db.Users.FirstOrDefault(x => x.Id == user.Id)
      ).ToArray();

      // Validando
      foreach (var user in users)
      {
        if (user == null)
          return Ret.Fail(HttpStatusCode.NotFound, $"A conta de usuário não existe.");

        var groupId = form.GroupId ?? user.GroupId;

        if (!Db.Groups.Any(x => x.Id == groupId))
          return Ret.Fail(HttpStatusCode.BadRequest, $"O grupo de usuário não existe.");
      }

      // Editando
      foreach (var user in users)
      {
        user.Enabled = form.Enabled ?? user.Enabled;
        user.GroupId = form.GroupId ?? user.GroupId;
        result.Add(user);
      }

      return result;
    }

    public Ret Delete(Bulk<User> users)
    {
      foreach (var user in users)
      {
        Db.Users.RemoveAll(x => x.Id == user.Id);
      }
      return Ret.OK();
    }
  }
}
