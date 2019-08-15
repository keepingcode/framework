import PaperText from '../../components/QPaperText.vue'
import PaperNumber from '../../components/QPaperNumber.vue'
import PaperCheckbox from '../../components/QPaperCheckbox.vue'
import PaperSelect from '../../components/QPaperSelect.vue'
import PaperHidden from '../../components/QPaperHidden.vue'
import PaperDatetime from '../../components/QPaperDatetime.vue'
import PaperDate from '../../components/QPaperDate.vue'
import PaperCurrency from '../../components/QPaperCurrency.vue'
import PaperUploader from '../../components/QPaperUploader.vue'

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

    this.rel = 'action'
  }

  get widgets () {
    if (this.hasForm()) {
      var entity = this.store.state.paper.entity
      var filter = entity.getSubEntityByRel(this.rel)
      var widgets = filter.getSubEntitiesByClass('widget')
      return widgets
    }
  }

  get links () {
    if (this.hasForm()) {
      var entity = this.store.state.paper.entity
      var form = entity.getSubEntityByRel(this.rel)
      var links = form.links.filter(link => link.class === undefined || !link.class.includes('widget'))
      return links
    }
  }

  getProperties (formName) {
    if (this.hasForm()) {
      var entity = this.store.state.paper.entity
      if (entity && entity.hasSubEntityByClass(formName)) {
        var links = entity.getSubEntitiesByClass(formName)
        var result = links.map(record => record.properties)
        return result
      }
      return links.properties
    }
  }

  hasForm () {
    var entity = this.store.state.paper.entity
    if (entity) {
      return entity.hasSubEntityByRel(this.rel)
    }
    return false
  }

  getForm (formName) {
    if (this.hasForm(formName)) {
      var entity = this.store.state.paper.entity
      var form = entity.getActionByName(formName)
      return form
    }
  }

  dynamicComponent (widget) {
    switch (widget.properties.type) {
      case this.type.HIDDEN:
        return PaperHidden
      case this.type.SELECT:
        return PaperSelect
      case this.type.DATETIME:
        return PaperDatetime
      case this.type.DATE:
        return PaperDate
      case this.type.FILE:
        return PaperUploader
      case this.type.TEXT:
        return this._dynamicComponent(widget)
      case this.type.NUMBER:
        return this._dynamicComponent(widget)
      default:
        return PaperText
    }
  }

  _dynamicComponent (widget) {
    switch (widget.properties.dataType) {
      case this.dataType.HIDDEN:
        return PaperHidden
      case this.dataType.DATETIME:
        return PaperDatetime
      case this.dataType.DECIMAL:
        return PaperCurrency
      case this.dataType.MULTI:
        return PaperSelect
      case this.dataType.NUMBER:
        return PaperNumber
      case this.dataType.BOOL:
        return PaperCheckbox
      case this.dataType.INT:
        return PaperNumber
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
