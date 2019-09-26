using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Diagnostics;

namespace Processa.Host
{
  /// <summary>
  /// Utilitário de autoinstalação do WebHost.
  /// </summary>
  public partial class WebHostInstaller : System.Configuration.Install.Installer
  {
    public WebHostInstaller()
    {
      try
      {

        this.fabricaDoHost = fabricaDoHost;
        InitializeComponent();

        MapearAssemblies();
        MapearConfiguracao();
        MapearParametros();

      }
      catch (Exception ex)
      {
        throw DumpException(ex, "Não foi possível inicializar o WebHostInstaller.");
      }
    }

    /// <summary>
    /// Instância do WebHost a ser instalada.
    /// Apenas os parâmetros são usados na prática.
    /// O WebHost jamais é lançado durante a instalação.
    /// No fim da instalação uma execução do serviço é acionada que irá
    /// produzir a criação de uma nova instância de WebHost pelo seu método
    /// Abrir*().
    /// </summary>
    private WebHost WebHost
    {
      get
      {
        if (webHost == null)
        {
          try
          {
            webHost = fabricaDoHost.Invoke();
          }
          catch (Exception ex)
          {
            throw DumpException(ex, "Não foi possível inicializar o WebHost.");
          }
        }
        return webHost;
      }
    }

    /// <summary>
    /// Durante a instalação o aplicativo de setup responsável por disparar a instalação do
    /// Host pode não estar sendo executado a partir da mesma pasta que o aplicativo executável
    /// do Host. Neste caso os assemblies que acompanham o Host podem ser lidos corretamente.
    /// 
    /// Para evitar o problema este método mapeia a pasta do Host e carrega os assemblies
    /// sob demanda conforma o andamento da instalação.
    /// </summary>
    private void MapearAssemblies()
    {
      try
      {

        var assembly = Assembly.GetAssembly(this.GetType());
        var executavel = Path.GetFullPath(assembly.Location);
        var pasta = Path.GetDirectoryName(executavel);

        var dominio = AppDomain.CurrentDomain;
        var diretorio = new DirectoryInfo(pasta);

        dominio.AssemblyResolve += (object sender, ResolveEventArgs args) =>
        {
          try
          {

            var module = diretorio.GetFiles().FirstOrDefault(x => x.Name == args.Name + ".dll");
            if (module != null)
            {
              var assemblyCarregado = Assembly.LoadFrom(module.FullName);
              return assemblyCarregado;
            }
            return null;

          }
          catch (Exception ex)
          {
            throw DumpException(ex, "Falhou a tentativa de executar o evento MapearAssemblies/AssemblyResolve.");
          }
        };

      }
      catch (Exception ex)
      {
        throw DumpException(ex, "Falhou a tentativa de executar o evento MapearAssemblies.");
      }
    }

    /// <summary>
    /// Durante a instalação o aplicativo executor da instalação pode não
    /// ser o mesmo Assembly que contém o ponto de entrada do serviço.
    ///
    /// As implementações atuais de WebConfig e HostConfig leem suas configurações
    /// do arquivo de configuração do executável.
    ///
    /// Vamos precisar instalar novas versões dos configuradores com base na
    /// instância do executável que conhecemos, aquele que contém a instância de
    /// WebHostInstaller atualmente em execução.
    /// </summary>
    private void MapearConfiguracao()
    {
      try
      {

        var assembly = Assembly.GetAssembly(this.GetType());
        var executavel = Path.GetFullPath(assembly.Location);
        var arquivoDeConfiguracao = executavel + ".config";

        if (!File.Exists(arquivoDeConfiguracao))
        {
          Console.WriteLine("[INFO]O serviço não está acompanhado de um arquivo de configuração: " + arquivoDeConfiguracao);
          return;
        }

        var mapa = new ExeConfigurationFileMap();
        mapa.ExeConfigFilename = arquivoDeConfiguracao;

        var configuracao = ConfigurationManager.OpenMappedExeConfiguration(mapa, ConfigurationUserLevel.None);

        HostConfig.Atual = configuracao.GetSection(HostConfig.NomeDaSecao) as HostConfig;
        WebConfig.Atual = configuracao.GetSection(WebConfig.NomeDaSecao) as WebConfig;

      }
      catch (Exception ex)
      {
        throw DumpException(ex, "Falhou a tentativa de executar o evento MapearConfiguracao.");
      }
    }

    /// <summary>
    /// Aplicar os parâmetros do Host aos parâmetros do instalador para garantir
    /// coesão nas configurações aplicadas.
    /// </summary>
    private void MapearParametros()
    {
      try
      {

        var host = this.WebHost;
        var parametros = host.ParametrosEfetivos;

        this.Installer.ServiceName = parametros.Nome;
        this.Installer.DisplayName = parametros.TituloDeServico;
        this.Installer.Description = parametros.Descricao;

      }
      catch (Exception ex)
      {
        throw DumpException(ex, "Falhou a tentativa de executar o evento MapearParametros.");
      }
    }

    /// <summary>
    /// Inicia o serviço recém instalado.
    /// </summary>
    private void IniciarServico()
    {
      try
      {

        var host = this.WebHost;
        var parametros = host.ParametrosEfetivos;

        ServiceController sc = new ServiceController(parametros.Nome);
        sc.Start();

      }
      catch (Exception ex)
      {
        throw DumpException(ex, "Falhou a tentativa de executar o evento IniciarServico.");
      }
    }

    /// <summary>
    /// Para o serviço instalado.
    /// </summary>
    private void PararServico()
    {
      try
      {

        var host = this.WebHost;
        var parametros = host.ParametrosEfetivos;

        ServiceController sc = new ServiceController(parametros.Nome);
        sc.Stop();

      }
      catch (Exception ex)
      {
        throw DumpException(ex, "Falhou a tentativa de executar o evento IniciarServico.");
      }
    }

    protected override void OnCommitted(IDictionary savedState)
    {
      base.OnCommitted(savedState);
      try
      {
        IniciarServico();
      }
      catch (Exception ex)
      {
        throw DumpException(ex, "Falhou a tentativa de executar o evento OnCommitted.");
      }
    }

    protected override void OnBeforeUninstall(IDictionary savedState)
    {
      try
      {
        PararServico();
      }
      catch (Exception ex)
      {
        DumpException(ex, "Falhou a tentativa de executar o evento OnBeforeUninstall.");
      }
      base.OnBeforeUninstall(savedState);
    }

    #region Logging para registro de desastres

    private string dumpFile;

    private string GetDumpFile()
    {
      if (dumpFile == null)
      {
        string appName = null;
        try
        {
          appName = webHost.ParametrosEfetivos.Nome;
        }
        catch
        {
          appName = "ProcessaHost";
        }

        var random = Process.GetCurrentProcess().Id;
        dumpFile = string.Format(
          "Processa_{0}_InstallAttempt_{1:yy-MM-dd}_{2:00000}.log"
          , appName, DateTime.Now, random
        );
      }
      return dumpFile;
    }

    private Exception DumpException(Exception exception, string message)
    {
      string trace = null;
      try
      {
        trace = exception.GetStackTrace();
        var file = GetDumpFile();
        File.AppendAllText(file, trace);

        message+=" \nLog: " + file;
      }
      catch
      {
        if (trace != null)
        {
          Trace.WriteLine(trace);
        }
      }
      return new Exception(message, exception);
    }

    #endregion

  }
}
