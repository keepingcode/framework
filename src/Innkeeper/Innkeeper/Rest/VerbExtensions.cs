
using System;
using System.Collections.Generic;
using System.Text;
using Toolset;

namespace Innkeeper.Rest
{
  public static class VerbExtensions
  {
    /// <summary>
    /// Obtém o nome padronizado do método HTTP.
    /// O nome obtido equivale àquele declarado nas constantes de
    /// MethodNames.
    /// </summary>
    /// <param name="verb">O método HTTP.</param>
    /// <returns>O nome padronizado do método HTTP</returns>
    public static string GetValue(this Verb verb)
    {
      if (verb == Verb.All) return VerbNames.All;
      return verb.ToString().ToUpper();
    }
  }
}