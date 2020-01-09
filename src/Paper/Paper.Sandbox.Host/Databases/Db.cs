using Paper.Sandbox.Host.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Sandbox.Host.Databases
{
  public static class Db
  {
    private static int lastId;

    public static int GenerateId() => ++lastId;

    public static List<User> Users = new List<User>
    {
      new User
      {
        Id = GenerateId(),
        Login = "Fulano",
        Name = "fulano",
        UserId = 1
      },
      new User
      {
        Id = GenerateId(),
        Login = "Beltrano",
        Name = "beltrano",
        UserId = 2
      },
      new User
      {
        Id = GenerateId(),
        Login = "Sicrano",
        Name = "sicrano",
        UserId = 2
      },
      new User
      {
        Id = GenerateId(),
        Login = "Alano",
        Name = "alano",
        UserId = 3
      },
      new User
      {
        Id = GenerateId(),
        Login = "Mengano",
        Name = "mengano",
        UserId = 3
      },
      new User
      {
        Id = GenerateId(),
        Login = "Zutano",
        Name = "zutano",
        UserId = 3
      },
      new User
      {
        Id = GenerateId(),
        Login = "Citano",
        Name = "citano",
        UserId = 3
      },
      new User
      {
        Id = GenerateId(),
        Login = "Perengano",
        Name = "perengano",
        UserId = 3
      }
    };

    public static List<Group> Groups = new List<Group>
    {
      new Group
      {
        Id = GenerateId(),
        Name = "Administrador"
      },
      new Group
      {
        Id = GenerateId(),
        Name = "Super Usuário"
      },
      new Group
      {
        Id = GenerateId(),
        Name = "Usuário"
      }
    };
  }
}
