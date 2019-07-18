import PaperText from '../../components/QPaperText.vue'
import PaperNumber from '../../components/QPaperNumber.vue'
import PaperCheckbox from '../../components/QPaperCheckbox.vue'
import PaperSelect from '../../components/QPaperSelect.vue'
import PaperHidden from '../../components/QPaperHidden.vue'
import PaperDatetime from '../../components/QPaperDatetime.vue'
import PaperCurrency from '../../components/QPaperCurrency.vue'
import PaperLabel from '../../components/QPaperLabel.vue'

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
    this.requester = new Requester(store, router)

    this.filtersActionName = '__filter'
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
  }

  dynamicComponent (field, actionName) {
    var headers = this.getProperties(actionName)
    switch (field.type) {
      case this.type.HIDDEN:
        return PaperHidden
      case this.type.DATETIME:
        return PaperDatetime
      case this.type.DATE:
        return PaperDatetime
      case this.type.TEXT:
        if (!headers) {
          return PaperText
        }
        return this._dynamicComponent(field, headers)
      case this.type.NUMBER:
        if (!headers) {
          return PaperNumber
        }
        return this._dynamicComponent(field, headers)
      default:
        return PaperText
    }
  }

  _dynamicComponent (field, headers) {
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

  async submit (form, link) {
    var params = {}
    if (form && form.length > 0) {
      for (var i = 0; i < form.length; i++) {
        var field = form[i]
        if (field && field.name && field.value) {
          params[field.name] = field.value
        }
      }
    }
    var page = link.href
    if (this.router.currentRoute.name === 'demo') {
      page = `/statics${page}.json`
    }
    console.log('page', page)
    console.log('params', params)
    return this.requester.requestSiren(page, params)
  }
}
