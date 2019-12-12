using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.CSharp;
using System.CodeDom;

namespace Paper.Media
{
  /// <summary>
  /// Classes conhecidas de entidades.
  /// </summary>
  public static class ClassNames
  {
    public const string Record = "record";

    public const string Action = "action";

    public const string Field = "field";

    //#region Estruturais básicas

    ///// <summary>
    ///// Nome de classe que representa um registro.
    ///// </summary>
    //public const string Record = "record";

    ///// <summary>
    ///// Nome de classe que representa dados de um formulário de edição.
    ///// </summary>
    //public const string Form = "form";

    ///// <summary>
    ///// Nome de classe para ume entidade que se comporta como um cabeçalho.
    ///// </summary>
    //public const string Header = "header";

    ///// <summary>
    ///// Nome de classe para uma entidade que se comporta como status de execução.
    ///// </summary>
    //public const string Status = "status";

    ///// <summary>
    ///// Nome de classe para uma entidade que se comporta como erro.
    ///// </summary>
    //public const string Error = "error";

    ///// <summary>
    ///// Nome de classe para uma ação que se comporta como filtro de lista.
    ///// </summary>
    //public const string Filter = "filter";

    ///// <summary>
    ///// Nome de classe para uma ação, entidade ou link que se comporta como um hiperlink.
    ///// </summary>
    //public const string Hyperlink = "hyperlink";

    ///// <summary>
    ///// Nome de classe para uma entidade que transporta um valor literal apenas, como um 
    ///// texto, número, etc.
    ///// </summary>
    //public const string Literal = "literal";

    ///// <summary>
    ///// Classe de uma entidade que representa um propriedade ou coluna de dados.
    ///// </summary>
    //public const string Field = "field";

    //#endregion

    //#region Estruturais avançadas

    ///// <summary>
    ///// Nome de classe para ume entidade que representa a configuração de um site.
    ///// </summary>
    //public const string Blueprint = "blueprint";

    ///// <summary>
    ///// Nome de classe para ume entidade que se comporta como uma coleção de registros.
    ///// </summary>
    //public const string Table = "table";

    ///// <summary>
    ///// Nome de classe para uma entidade que se comporta como lista.
    ///// </summary>
    //public const string List = "list";

    ///// <summary>
    ///// Nome de classe para uma entidade que representa um item de lista.
    ///// </summary>
    //public const string Item = "item";

    ///// <summary>
    ///// Nome de classe para ume entidade que se comporta como uma coleção de cards.
    ///// </summary>
    //public const string Cards = "cards";

    ///// <summary>
    ///// Nome de classe para ume entidade que se comporta como um registro de coleção de cards.
    ///// </summary>
    //public const string Card = "card";

    //#endregion

    //#region Extensões

    //#endregion
  }
}