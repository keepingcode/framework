using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Toolset;
using Toolset.Serialization;
using Toolset.Serialization.Graph;
using System.Collections;
using Toolset.Collections;
using Toolset.Reflection;

namespace Paper.Media
{
  [DataContract(Namespace = Namespaces.Default)]
  [KnownType(typeof(FieldValueCollection))]
  [KnownType(typeof(CaseVariantString))]
  public class Field : IPropertyValueSetter
  {
    public static readonly string[] FieldValuePropertyNames;

    private string _type;
    private string _dataType;
    private string _title;
    private bool? _readOnly;

    static Field()
    {
      FieldValuePropertyNames = typeof(FieldValue)._GetPropertyNames().ToArray();
    }

    /// <summary>
    /// Nome do campo.
    /// </summary>
    [DataMember(EmitDefaultValue = false, Order = 10)]
    [CaseVariantString]
    public virtual string Name { get; set; }

    /// <summary>
    /// Tipo do componente de edição do campo.
    /// 
    /// É aceito qualquer um dos tipos convencionados para o HTML5:
    /// - hidden
    /// - text
    /// - search
    /// - tel
    /// - url
    /// - email
    /// - password
    /// - datetime
    /// - date
    /// - month
    /// - week
    /// - time
    /// - datetime-local
    /// - number
    /// - range
    /// - color
    /// - checkbox
    /// - radio
    /// - file
    /// 
    /// A lista completa está definida na classe FieldTypeNames.
    /// </summary>
    [DataMember(EmitDefaultValue = false, Order = 20)]
    public virtual string Type
    {
      get => _type;
      set => _type = value;
    }

    /// <summary>
    /// Título do campo. Opcional.
    /// </summary>
    [DataMember(EmitDefaultValue = false, Order = 30)]
    public virtual string Title
    {
      get
      {
        if (_title != null)
          return _title;
        if (!string.IsNullOrEmpty(Name))
          return Name.ChangeCase(TextCase.ProperCase);
        return null;
      }
      set => _title = value;
    }

    /// <summary>
    /// Valor do campo. Opcional.
    /// Um valor pode ser um dos tipos básicos do C# ou uma coleção
    /// FieldValueCollection.
    /// </summary>
    [DataMember(EmitDefaultValue = false, Order = 40)]
    public virtual object Value { get; set; }

    /// <summary>
    /// Torna o campo editável ou somente leitura.
    /// </summary>
    [DataMember(Name = "Readonly", EmitDefaultValue = false, Order = 45)]
    public virtual bool? ReadOnly
    {
      get => (Type == FieldTypeNames.Hidden) ? true : _readOnly;
      set => _readOnly = value;
    }

    #region Toolset.Serialization.Graph.IPropertyValueSetter

    /// <summary>
    /// Método de deserialização de propriedade.
    /// 
    /// O deserializador invoca este método durante a construção do objeto
    /// para cada propriedade deserializada.
    /// 
    /// O documento (document) repassado contém a estrutura hierárquica das propriedades
    /// deserializadas.
    /// 
    /// O construtor de objetos (builder) pode ser usado para construir automaticamente
    /// uma objeto de um tipo qualquer a partir do documento ou parte dele.
    /// 
    /// Caso o valor da propriedade tenha sido definido o método deve retornar verdadeiro.
    /// O deserializador entende a propriedade como definida e avança na deserialização.
    /// 
    /// Caso falso seja retornado o deserializador tenta outras formas de definir o
    /// valor da propriedade.
    /// </summary>
    /// <param name="property">O nome da propriedade deserializada.</param>
    /// <param name="document">
    /// O documento contendo a estrutura hierárquica das propriedades deserializada.
    /// </param>
    /// <param name="builder">O construtor de objetos.</param>
    /// <returns>
    /// Verdadeiro caso o valor da propriedade tenha sido definido;
    /// Falso caso contrário.
    /// </returns>
    public bool SetProperty(string property, NodeModel document, GraphBuilder builder)
    {
      if (property == nameof(Value))
      {
        // O valor de um campo pode ser:
        // -  Um simples valor, como string ou inteiro.
        // -  Uma coleção de valores selecionáveis, do tipo FieldValueCollection
        // -  Um objeto complexo, do tipo PropertyMap.

        if (Type == FieldTypeNames.Select)
        {
          var fields = new FieldValueCollection();
          foreach (var child in document.Children())
          {
            var field = builder.CreateGraph<FieldValue>(child);
            fields.Add(field);
          }
          this.Value = fields;
          return true;
        }

        if (document is ValueModel valueModel)
        {
          this.Value = valueModel.Value;
          return true;
        }

        if (document is ObjectModel objectModel)
        {
          var graph = builder.CreateGraph<PropertyMap>(document);
          this.Value = new PropertyMap[] { graph };
          return true;
        }

        if (document is CollectionModel collectionModel)
        {
          var graphs = new List<PropertyMap>();
          foreach (var child in document.Children())
          {
            var graph = builder.CreateGraph<PropertyMap>(child);
            graphs.Add(graph);
          }
          this.Value = graphs.ToArray();
          return true;
        }
      }

      return false;
    }

    #endregion
  }
}

