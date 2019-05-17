using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Toolset;

namespace Paper.Browser.Web
{
  /// <summary>
  /// Utilitário de leitura dos arquivos embarcados no assembly.
  /// </summary>
  public class ResourceLoader
  {
    /*
     * Observação:
     * 
     * Para que a leitura de recursos funcione como esperado os arquivos
     * devem ser embarcados com seus caminhos de pastas reais.
     * 
     * Por exemplo:
     *    \Pasta\Arquivo.tal
     * 
     * Para que o compilador respeite esta regra é necessário adicionar uma
     * configuração como esta no arquivo *.csproj:
     * 
     *    <ItemGroup>
     *      <EmbeddedResource Include="site\dist\**">
     *        <LogicalName>\%(RecursiveDir)%(Filename)%(Extension)</LogicalName>
     *      </EmbeddedResource>
     *    </ItemGroup>
     */

    /// <summary>
    /// Coleção dos caminhos dos arquivos embarcados no Assembly.
    /// </summary>
    public string[] ResourcePaths => typeof(ResourceLoader).Assembly.GetManifestResourceNames();

    /// <summary>
    /// Localizar os nomes de arquivos que conferem com o filtro indicado.
    /// </summary>
    /// <param name="pathFilter">
    /// Filtro para pesquisa de arquivos.
    /// 
    /// São suportados os caracteres curinga:
    /// *, para indicar um trecho de nome, **, para indicar qualquer quantidade
    /// de pastas no caminho, e ?, para indicar um único caracter na posição.
    /// 
    /// Por exemplo: O filtro "\Path\*.txt" retorna todos os arquivos TXT dentro da
    /// pasta "Path". O filtro "\Path\**\*.txt" retorna todos os arquivos TXT dentro
    /// da pasta "Path" ou em qualquer outra pasta na hierarquia de "Path".
    /// </param>
    /// <returns>
    /// O status da pesquisa mais os arquivos lidos.
    /// Em geral 200-OK com a lista dos arquivos lidos ou 404-NotFound com uma
    /// lista vazia caso nada seja encontrado.
    /// </returns>
    public Ret<string[]> FindResources(string pathFilter)
    {
      try
      {
        pathFilter = pathFilter.Replace(@"/", @"\");

        if (!pathFilter.StartsWith(@"\"))
        {
          pathFilter = $@"\**\{pathFilter}";
        }

        var pattern = pathFilter
          .Replace(@".", @"[.]")
          .Replace(@"**", @"§")
          .Replace(@"*", @"[^\]*")
          .Replace(@"\§", @"(|\.*)")
          .Replace(@"?", @".")
          .Replace(@"\", @"\\")
          ;

        var regex = new Regex($"^{pattern}$", RegexOptions.IgnoreCase);

        var files = (
          from path in ResourcePaths
          where regex.IsMatch(path)
          select path
        ).ToArray();

        return files.Length > 0 ? Ret.OK(files) : Ret.NotFound(files);
      }
      catch (Exception ex)
      {
        return Ret.FailWithValue(ex, new string[0]);
      }
    }

    /// <summary>
    /// Copia o arquivo embarcado para o fluxo de saída.
    /// </summary>
    /// <param name="resourcePath">
    /// O caminho do recurso procurado, com pastas separadas por contra-barra (\).
    /// Exemplo: "\My\Folder\MyFile.txt"
    /// </param>
    /// <param name="output">Fluxo para escrita do arquivo embarcado.</param>
    /// <returns>
    /// O status de leitura do arquivo.
    /// Em geral 200-OK caso o arquivo seja lido com sucesso
    /// ou 404-NotFound caso o arquivo não exista embarcado.
    /// </returns>
    public Ret LoadResource(string resourcePath, Stream output)
    {
      try
      {
        string realPath;

        realPath = resourcePath.Replace(@"/", @"\");
        realPath = ResourcePaths.FirstOrDefault(x => x.EqualsIgnoreCase(realPath));

        if (realPath == null)
        {
          return Ret.NotFound();
        }

        var assembly = typeof(ResourceLoader).Assembly;
        using (var input = assembly.GetManifestResourceStream(realPath))
        {
          input.CopyTo(output);
        }

        return Ret.OK();
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    /// <summary>
    /// Copia o arquivo embarcado para o fluxo de saída.
    /// </summary>
    /// <param name="resourcePath">
    /// O caminho do recurso procurado, com pastas separadas por contra-barra (\).
    /// Exemplo: "\My\Folder\MyFile.txt"
    /// </param>
    /// <param name="writer">Fluxo para escrita do arquivo embarcado.</param>
    /// <returns>
    /// O status de leitura do arquivo.
    /// Em geral 200-OK caso o arquivo seja lido com sucesso
    /// ou 404-NotFound caso o arquivo não exista embarcado.
    /// </returns>
    public Ret LoadResource(string resourcePath, TextWriter writer)
    {
      try
      {
        string realPath;
        
        realPath = resourcePath.Replace(@"/", @"\");
        realPath = ResourcePaths.FirstOrDefault(x => x.EqualsIgnoreCase(realPath));

        if (realPath == null)
        {
          return Ret.NotFound();
        }

        var assembly = typeof(ResourceLoader).Assembly;
        using (var input = assembly.GetManifestResourceStream(realPath))
        {
          var reader = new StreamReader(input);
          reader.CopyTo(writer);
        }

        return Ret.OK();
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    /// <summary>
    /// Retorna o conteúdo do arquivo texto embarcado.
    /// </summary>
    /// <param name="resourcePath">
    /// O caminho do recurso procurado, com pastas separadas por contra-barra (\).
    /// Exemplo: "\My\Folder\MyFile.txt"
    /// </param>
    /// <returns>
    /// O status de leitura do arquivo mais o seu conteúdo.
    /// Em geral 200-OK mais o conteúdo caso o arquivo seja lido com sucesso
    /// ou 404-NotFound caso o arquivo não exista embarcado.
    /// </returns>
    public Ret<string> LoadResourceAsText(string resourcePath)
    {
      try
      {
        using (var writer = new StringWriter())
        {
          var ret = LoadResource(resourcePath, writer);
          if (!ret.Ok)
          {
            return ret;
          }

          var content = writer.ToString();
          return Ret.OK(content);
        }
      }
      catch (Exception ex)
      {
        return ex;
      }
    }

    /// <summary>
    /// Retorna o conteúdo do arquivo binário embarcado.
    /// </summary>
    /// <param name="resourcePath">
    /// O caminho do recurso procurado, com pastas separadas por contra-barra (\).
    /// Exemplo: "\My\Folder\MyFile.txt"
    /// </param>
    /// <returns>
    /// O status de leitura do arquivo mais o seu conteúdo.
    /// Em geral 200-OK mais o conteúdo caso o arquivo seja lido com sucesso
    /// ou 404-NotFound caso o arquivo não exista embarcado.
    /// </returns>
    public Ret<byte[]> LoadResourceAsBinary(string resourcePath)
    {
      try
      {
        using (var memory = new MemoryStream())
        {
          var ret = LoadResource(resourcePath, memory);
          if (!ret.Ok)
          {
            return ret;
          }

          var bytes = memory.ToArray();
          return Ret.OK(bytes);
        }
      }
      catch (Exception ex)
      {
        return ex;
      }
    }
  }
}
