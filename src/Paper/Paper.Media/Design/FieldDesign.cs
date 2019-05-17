using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Paper.Media.Data;

namespace Paper.Media.Design
{
  public class FieldDesign : PropertyDesign, IFieldInfo
  {
    public FieldDesign(Entity fieldEntity, EntityAction action)
      : base(fieldEntity)
    {
      this.FieldEntity = fieldEntity;
      this.ProviderKeys =
        new ProviderKeyCollectionAdapter(() => EnsurePropertyValueCollection(nameof(ProviderKeys)));

      fieldEntity.AddClass(ClassNames.Field);
      fieldEntity.AddRel(RelNames.Action);
      fieldEntity.AddRel(action.Name);
    }

    private PropertyValueCollection EnsurePropertyValueCollection(string property)
    {
      var collection = FieldEntity.Properties[property] as PropertyValueCollection;
      if (collection == null)
      {
        FieldEntity.Properties[property] = new PropertyValueCollection();
        collection = FieldEntity.Properties[property] as PropertyValueCollection;
      }
      return collection;
    }

    public Entity FieldEntity { get; set; }

    /// <summary>
    /// Tipo do componente de usuário.
    /// </summary>
    public string ComponentType
    {
      get => Get<string>();
      set => Set<string>(value);
    }

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
    public string DataType
    {
      get => Get<string>();
      set => Set<string>(value);
    }

    /// <summary>
    /// Nome da categoria do campo para criação de agrupamentos.
    /// </summary>
    public string Category
    {
      get => Get<string>();
      set => Set<string>(value);
    }

    /// <summary>
    /// Texto adicional com uma breve instrução de uso do widget.
    /// </summary>
    public string Placeholder
    {
      get => Get<string>();
      set => Set<string>(value);
    }

    /// <summary>
    /// URL de referência do provedor de dados.
    /// </summary>
    public Href ProviderHref
    {
      get => Get<Href>();
      set => Set<Href>(value);
    }

    /// <summary>
    /// Nome das chaves de relacionamento entre o dado e o campo.
    /// </summary>
    public ProviderKeyCollectionAdapter ProviderKeys { get; }

    /// <summary>
    /// Ativa ou desativa a obrigatoriedade de preenchimento do campo.
    /// </summary>
    public bool? Required
    {
      get => Get<bool?>();
      set => Set<bool?>(value);
    }

    /// <summary>
    /// Tamanho mínimo para um texto ou menor valor para um número.
    /// </summary>
    public int? MinLength
    {
      get => Get<int?>();
      set => Set<int?>(value);
    }

    /// <summary>
    /// Tamanho máximo para um texto ou maior valor para um número.
    /// </summary>
    public int? MaxLength
    {
      get => Get<int?>();
      set => Set<int?>(value);
    }

    /// <summary>
    /// Expressão regular para validação do conteúdo de um campo texto.
    /// A expressão deve seguir a mesma forma aplicada para restrição de
    /// texto no XSD (Esquema de XML).
    /// Referências:
    /// - https://www.regular-expressions.info/xml.html
    /// - http://www.xmlschemareference.com/regularExpression.html
    /// </summary>
    public string Pattern
    {
      get => Get<string>();
      set => Set<string>(value);
    }

    /// <summary>
    /// Ativa ou desativa a edição em múltiplas linhas, geralmente para campos texto.
    /// </summary>
    public bool? Multiline
    {
      get => Get<bool?>();
      set => Set<bool?>(value);
    }

    /// <summary>
    /// Ativa ou desativa a múltipla seleção de valores para o campo.
    /// </summary>
    public bool? AllowMany
    {
      get => Get<bool?>();
      set => Set<bool?>(value);
    }

    /// <summary>
    /// Ativa ou desativa o suporte a intervalo, na forma "{ min=x, max=y }".
    /// </summary>
    public bool? AllowRange
    {
      get => Get<bool?>();
      set => Set<bool?>(value);
    }

    /// <summary>
    /// Ativa ou desativa o suporte aos curingas "*", para indicar qualquer texto
    /// na posição, e "?", para indicar qualquer caracter na posição.
    /// </summary>
    public bool? AllowWildcard
    {
      get => Get<bool?>();
      set => Set<bool?>(value);
    }
  }
}
