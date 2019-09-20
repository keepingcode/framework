using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Processa.Host
{
  /// <summary>
  /// Utilitário de autoinstalação do WebHost.
  /// </summary>
  /// <typeparam name="T">O tipo do WebHost.</typeparam>
  public class WebHostInstaller<T> : WebHostInstaller
    where T : WebHost, new()
  {
    public WebHostInstaller()
      : base(new T())
    {
    }
  }

}
