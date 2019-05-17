using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Media.Design
{
  public interface IFieldInfo
  {
    /// <summary>
    /// Tipo do componente de usuário.
    /// </summary>
    string ComponentType { get; }

    /// <summary>
    /// Tipo do valor do campo.
    /// 
    /// Tipos aceitos:
    /// - text (string)
    /// - bit (bool, boolean)
    /// - number (int, long)
    /// - decimal (double, float)
    /// - date
    /// - time
    /// - datetime
    /// 
    /// O texto em parêntesis representa um nome alternativo, ou apelido, para o tipo.
    /// 
    /// A lista completa está definida na classe FieldDataTypeNames.
    /// </summary>
    string DataType { get; }

    /// <summary>
    /// Nome da categoria do campo para criação de agrupamentos.
    /// </summary>
    string Category { get; }

    /// <summary>
    /// Texto adicional com uma breve instrução de uso do widget.
    /// </summary>
    string Placeholder { get; }

    /// <summary>
    /// URL de referência do provedor de dados.
    /// </summary>
    Href ProviderHref { get; }

    /// <summary>
    /// Nome das chaves de relacionamento entre o dado e o campo.
    /// </summary>
    ProviderKeyCollectionAdapter ProviderKeys { get; }

    /// <summary>
    /// Ativa ou desativa a obrigatoriedade de preenchimento do campo.
    /// </summary>
    bool? Required { get; }

    /// <summary>
    /// Tamanho mínimo para um texto ou menor valor para um número.
    /// </summary>
    int? MinLength { get; }

    /// <summary>
    /// Tamanho máximo para um texto ou maior valor para um número.
    /// </summary>
    int? MaxLength { get; }

    /// <summary>
    /// Expressão regular para validação do conteúdo de um campo texto.
    /// A expressão deve seguir a mesma forma aplicada para restrição de
    /// texto no XSD (Esquema de XML).
    /// Referências:
    /// - https://www.regular-expressions.info/xml.html
    /// - http://www.xmlschemareference.com/regularExpression.html
    /// </summary>
    string Pattern { get; }

    /// <summary>
    /// Ativa ou desativa a edição em múltiplas linhas, geralmente para campos texto.
    /// </summary>
    bool? Multiline { get; }

    /// <summary>
    /// Ativa ou desativa a múltipla seleção de valores para o campo.
    /// </summary>
    bool? AllowMany { get; }

    /// <summary>
    /// Ativa ou desativa o suporte a intervalo, na forma "{ min=x, max=y }".
    /// </summary>
    bool? AllowRange { get; }

    /// <summary>
    /// Ativa ou desativa o suporte aos curingas "*", para indicar qualquer texto
    /// na posição, e "?", para indicar qualquer caracter na posição.
    /// </summary>
    bool? AllowWildcard { get; }
  }
}
