using Paper.Design;
using Paper.Sandbox.Host.Databases;
using Paper.Sandbox.Host.Domain;
using Paper.Sandbox.Host.Filters;
using Paper.Sandbox.Host.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Toolset;
using Toolset.Reflection;

namespace Paper.Sandbox.Host.Business
{
  public class GroupBusiness
  {
    public Ret<ICollection<Group>> Find(Page page, Sort sort, GroupFilter filter)
    {
      return Db.Groups;
    }

    public Ret Create(GroupForm form)
    {
      var currentGroup = Db.Groups.FirstOrDefault(x => x.Name == form.Name);
      if (currentGroup != null)
        return Ret.Fail(HttpStatusCode.Conflict, $"Já existe um grupo de usuário registrado para este nome: {form.Name}");

      Db.Groups.Add(
        new Group
        {
          Id = Db.GenerateId(),
          Name = form.Name
        }
      );

      return Ret.OK();
    }

    public Ret Edit(GroupForm form, ICollection<Group> affectedGroups)
    {
      var currentGroup = affectedGroups.FirstOrDefault(x => Db.Groups.Any(y => y.Name == x.Name));
      if (currentGroup == null)
        return Ret.Fail(HttpStatusCode.NotFound, $"Não existe um grupo de usuário registrado para este nome: {currentGroup.Name}");

      var groups =
        from affectedGroup in affectedGroups
        join @group in Db.Groups
          on affectedGroup.Name equals @group.Name
        select @group;

      foreach (var group in groups)
      {
        group.Name = form.Name ?? group.Name;
      }

      return Ret.OK();
    }
  }
}
