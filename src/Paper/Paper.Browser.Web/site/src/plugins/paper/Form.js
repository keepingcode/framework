import PaperText from '../../components/PaperText.vue'
import PaperNumber from '../../components/PaperNumber.vue'
import PaperCheckbox from '../../components/PaperCheckbox.vue'
import PaperSelect from '../../components/PaperSelect.vue'
import PaperHidden from '../../components/PaperHidden.vue'
import PaperDatetime from '../../components/PaperDatetime.vue'
import PaperCurrency from '../../components/PaperCurrency.vue'
import PaperLabel from '../../components/PaperLabel.vue'

import Type from './Type.js'
import DataType from './DataType.js'
import Requester from './Requester.js'

export default class Form {
  constructor (options, router, store, vue) {
    this.vue = vue
    this.options = options
    this.dataType = new DataType()
    this.type = new Type()
    this.router = router
    this.store = store
    this.requester = new Requester(router)

    this.filtersActionName = '__filters'
  }

  hasFilters () {
    return this.hasForm(this.filtersActionName)
  }

  hasForm (formName) {
    var entity = this.store.state.paper.entity
    if (entity) {
      return entity.hasActionByName(formName)
    }
    return false
  }

  getFilters () {
    return this.getForm(this.filtersActionName)
  }

  getForm (formName) {
    if (this.hasForm(formName)) {
      var entity = this.store.state.paper.entity
      var form = entity.getActionByName(formName)
      return form
    }
  }

  getFiltersProperties () {
    return this.getProperties(this.filtersActionName)
  }

  getProperties (formName) {
    var entity = this.store.state.paper.entity
    if (entity && entity.hasSubEntityByClass(formName)) {
      var filters = entity.getSubEntitiesByClass(formName)
      var result = filters.map(record => record.properties)
      return result
    }
    return []
  }

  dynamicComponent (field, actionName) {
    switch (field.type) {
      case this.type.HIDDEN:
        return PaperHidden
      case this.type.DATETIME:
        return PaperDatetime
      case this.type.TEXT:
        return this._dynamicComponent(field, actionName)
      case this.type.NUMBER:
        return this._dynamicComponent(field, actionName)
      default:
        return PaperText
    }
  }

  _dynamicComponent (field, actionName) {
    var headers = this.getProperties(actionName)
    if (!headers) {
      return PaperText
    }
    var item = headers.find(header => header.name === field.name)
    if (!item) {
      return PaperText
    }
    switch (item.dataType) {
      case this.dataType.HIDDEN:
        return PaperHidden
      case this.dataType.DATETIME:
        return PaperDatetime
      case this.dataType.STRING:
        return PaperLabel
      case this.dataType.DECIMAL:
        return PaperCurrency
      case this.dataType.MULTI:
        return PaperSelect
      case this.dataType.NUMBER:
        return PaperNumber
      case this.dataType.BOOL:
        return PaperCheckbox
      default:
        return PaperText
    }
  }

  submit (action, form) {
    var params = {}
    if (form && form.length > 0) {
      for (var i = 0; i < form.length; i++) {
        var field = form[i]
        if (field && field.name && field.value) {
          params[field.name] = field.value
        }
      }
    }
    console.log('params', params)
    this.requester.openUrl(action.href, params)
  }
}
