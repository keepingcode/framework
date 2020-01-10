using Paper.Sandbox.Host.Demo.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Sandbox.Host.Demo
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
        GroupId = 1
      },
      new User
      {
        Id = GenerateId(),
        Login = "Beltrano",
        Name = "beltrano",
        GroupId = 2
      },
      new User
      {
        Id = GenerateId(),
        Login = "Sicrano",
        Name = "sicrano",
        GroupId = 2
      },
      new User
      {
        Id = GenerateId(),
        Login = "Alano",
        Name = "alano",
        GroupId = 3
      },
      new User
      {
        Id = GenerateId(),
        Login = "Mengano",
        Name = "mengano",
        GroupId = 3
      },
      new User
      {
        Id = GenerateId(),
        Login = "Zutano",
        Name = "zutano",
        GroupId = 3
      },
      new User
      {
        Id = GenerateId(),
        Login = "Citano",
        Name = "citano",
        GroupId = 3
      },
      new User
      {
        Id = GenerateId(),
        Login = "Perengano",
        Name = "perengano",
        GroupId = 3
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
