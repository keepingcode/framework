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
  public class UserBusiness
  {
    public Ret<ICollection<User>> Find(Page page, Sort sort, UserFilter filter)
    {
      return Db.Users;
    }

    public Ret Create(UserForm.Create form)
    {
      var currentUser = Db.Users.FirstOrDefault(x => x.Login == form.Login);
      if (currentUser != null)
        return Ret.Fail(HttpStatusCode.Conflict, $"Já existe uma conta registrada para este login: {form.Login}");

      var currentUser = Db.Users.FirstOrDefault(x => x.Name == form.User);
      if (currentUser == null)
        return Ret.Fail(HttpStatusCode.Conflict, $"O grupo de usuário não existe: {form.User}");

      Db.Users.Add(
        new User
        {
          Id = Db.GenerateId(),
          Login = form.Login,
          Name = form.Name,
          Enabled = (form.Enabled == true),
          UserId = currentUser.Id
        }
      );

      return Ret.OK();
    }

    public Ret Edit(UserForm.Edit form, ICollection<User> affectedUsers)
    {
      var currentUser = affectedUsers.FirstOrDefault(x => Db.Users.Any(y => y.Login == x.Login));
      if (currentUser == null)
        return Ret.Fail(HttpStatusCode.NotFound, $"Não existe uma conta registrada para este login: {currentUser.Login}");

      var group = Db.Users.FirstOrDefault(x => x.Name == form.User);

      var users =
        from affectedUser in affectedUsers
        join user in Db.Users
          on affectedUser.Login equals user.Login
        select user;

      foreach (var user in users)
      {
        user.Name = form.Name ?? user.Name;
        user.Enabled = form.Enabled ?? user.Enabled;
        user.UserId = group?.Id ?? user.UserId;
      }

      return Ret.OK();
    }
  }
}
