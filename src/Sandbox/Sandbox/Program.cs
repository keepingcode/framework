using Innkeeper.Rest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Toolset;
using Toolset.Collections;
using Toolset.Xml;

namespace Sandbox
{
  //public interface IUrl : ICloneable
  //{
  //  string Protocol { get; set; }

  //  string Host { get; set; }

  //  int? Port { get; set; }

  //  string Path { get; set; }

  //  IMap<string, Var> Args { get; }

  //  IUrl Combine(params string[] fragments);

  //  IUrl Append(params string[] fragments);

  //  IUrl Replace(params string[] fragments);

  //  IUrl ClearArgs();

  //  IUrl SetArg(string argName, object value);

  //  IUrl SetArgs(params object[] argValuePairs);

  //  new IUrl Clone();
  //}

  public class Url : ICloneable
  {
    private static readonly Regex namePattern = new Regex(@"^[^:/@]*$");
    private static readonly Regex pathPattern = new Regex(@"^(/[^/?]+)+/?$");

    private string _protocol;
    private string _host;
    private string _user;
    private string _pass;
    private string _path;

    public Url()
    {
    }

    public Url(string url)
    {
      Replace(url);
    }

    public string Protocol
    {
      get => string.IsNullOrEmpty(_protocol) ? null : _protocol;
      set
      {
        if (!string.IsNullOrEmpty(value) && !namePattern.IsMatch(value))
          throw new Exception($"O texto informado não corresponde a um protocolo válido: {value}");

        _protocol = value;
      }
    }

    public string Host
    {
      get => string.IsNullOrEmpty(_host) ? null : _host;
      set
      {
        if (!string.IsNullOrEmpty(value) && !namePattern.IsMatch(value))
          throw new Exception($"O texto informado não corresponde a um host ou IP válido: {value}");

        _host = value;
      }
    }

    public string User
    {
      get => string.IsNullOrEmpty(_user) ? null : _user;
      set
      {
        if (!string.IsNullOrEmpty(value) && !namePattern.IsMatch(value))
          throw new Exception($"O texto informado não corresponde a um nome de usuário válido: {value}");

        _user = value;
      }
    }

    public string Pass
    {
      get => string.IsNullOrEmpty(_pass) ? null : _pass;
      set
      {
        if (!string.IsNullOrEmpty(value) && !namePattern.IsMatch(value))
          throw new Exception($"O texto informado não corresponde a uma senha válida: {value}");

        _pass = value;
      }
    }

    public int? Port { get; set; }

    public string Path
    {
      get => string.IsNullOrEmpty(_path) ? null : _path;
      set
      {
        if (!string.IsNullOrEmpty(value) && !pathPattern.IsMatch(value))
          throw new Exception($"O texto informado não corresponde a um caminho válido: {value}");

        _path = value;
      }
    }

    public IMap<string, Var> Args { get; } = new HashMap<Var>();

    public Url Clone()
    {
      var url = new Url();
      url.Protocol = this.Protocol;
      url.Host = this.Host;
      url.Port = this.Port;
      url.Path = this.Path;
      if (this.Args != null)
      {
        url.Args.AddMany(this.Args);
      }
      return url;
    }

    object ICloneable.Clone()
    {
      return Clone();
    }

    public Url Combine(params string[] fragments)
    {
      throw new NotImplementedException();
    }

    public Url Append(params string[] fragments)
    {
      throw new NotImplementedException();
    }

    public Url Replace(params string[] fragments)
    {
      throw new NotImplementedException();
    }

    public Url ClearArgs()
    {
      Args?.Clear();
      return this;
    }

    public Url SetArg(string argName, object value)
    {
      if (value == null)
      {
        Args.Remove(argName);
      }
      else
      {
        Args[argName] = value as Var ?? new Var(value);
      }
      return this;
    }

    public Url SetArgs(params object[] argValuePairs)
    {
      if ((argValuePairs.Length % 2) != 2)
        throw new Exception("Era esperado um vetor com pares de parâmetros. Cada índice par como um nome de parâmetro e cada índice ímpar como valor do parâmetro. Ex: [ \"id\", 1, \"nome\", \"Fulano\" ]");

      for (var i = 0; i < argValuePairs.Length; i += 2)
      {
        var key = argValuePairs[i]?.ToString();
        var value = argValuePairs[i + 1];
        SetArg(key, value);
      }

      return this;
    }

    public override string ToString()
    {
      var builder = new StringBuilder();

      var path = Path;

      if (Protocol != null || Host != null || Port != null || User != null || Pass != null)
      {
        var prot = Protocol ?? "*";
        var host = Host ?? "*";

        string port;
        string credentials = null;

        if (Port == null)
          port = "";
        else if (Port == 0)
          port = ":*";
        else
          port = $":{Port}";

        if (User != null || Pass != null)
        {
          var user = User ?? "*";
          var pass = (Pass != null) ? $":{Pass}" : "";
          credentials = $"{user}{pass}@";
        }

        builder.Append(prot).Append("://").Append(credentials).Append(host).Append(port);

        if (string.IsNullOrEmpty(path))
        {
          path = "/";
        }
      }

      builder.Append(path);

      if (Args?.Any() == true)
      {
        var separator = "";

        builder.Append("?");
        foreach (var entry in Args)
        {
          var key = entry.Key;
          var var = entry.Value;

          if (var == null || var.IsNull)
            continue;

          if (var.IsValue)
          {
            builder.Append(separator).Append(key);
            if (var.Value is bool bit)
            {
              builder.Append("=").Append(bit ? "1" : "0");
            }
            else
            {
              var text = Change.To<string>(var.Value);
              builder.Append("=").Append(text);
            }
            separator = "&";
          }
          else if (var.IsRange)
          {
            if (var.Range.IsMinSet)
            {
              var text = Change.To<string>(var.Range.Min);
              builder.Append(separator).Append(key).Append(".min=").Append(text);
              separator = "&";
            }
            if (var.Range.IsMaxSet)
            {
              var text = Change.To<string>(var.Range.Max);
              builder.Append(separator).Append(key).Append(".max=").Append(text);
              separator = "&";
            }
          }
          else if (var.IsArray && var.Array.Count > 0)
          {
            foreach (var value in var.Array)
            {
              var text = Change.To<string>(value);
              builder.Append(separator).Append(key).Append("[]=").Append(text);
              separator = "&";
            }
          }
        }
      }

      return builder.ToString();
    }

    public static explicit operator string(Url url)
    {
      return url?.ToString();
    }

    public static explicit operator Url(string url)
    {
      return new Url(url);
    }
  }

  public static class Program
  {
    public static void Main(string[] commandArgs)
    {
      try
      {
        var url = new Url();

        url.Protocol = "http";
        url.Host = "10.0.0.1";
        url.Port = 090;
        url.User = "xyz";
        url.Pass = "123";

        url.Args["page"] = new Var(new Toolset.Range(10, 30));
        url.Args["oneShot"] = new Var(true);
        url.Args["search"] = new Var("Tananana");
        url.Args["ids"] = new Var(new[] { 1, 2, 3 });

        Debug.WriteLine(url);

        //var other =
        //  url
        //    .Clone()
        //    .ClearArgs()
        //    .Append("/Talz")
        //    .Combine("/Talz")
        //    .Append("?a[]=1")
        //    .Combine("?a[]=2")
        //    .Replace("*//local:*/");

      }
      catch (Exception ex)
      {
        ex.Trace();
      }
    }
  }
}