import PaperText from '../../components/QPaperText.vue'
import PaperNumber from '../../components/QPaperNumber.vue'
import PaperCheckbox from '../../components/QPaperCheckbox.vue'
import PaperSelect from '../../components/QPaperSelect.vue'
import PaperHidden from '../../components/QPaperHidden.vue'
import PaperDatetime from '../../components/QPaperDatetime.vue'
import PaperCurrency from '../../components/QPaperCurrency.vue'

import Type from './Type.js'
import DataType from './DataType.js'
import Requester from './Requester.js'
import Form from './Form.js'

export default class Filter {
  constructor (options, router, store, vue) {
    this.vue = vue
    this.options = options
    this.dataType = new DataType()
    this.type = new Type()
    this.router = router
    this.store = store
    this.requester = new Requester(store, router)
    this.form = new Form(options, router, store, vue)

    this.rel = 'filter'
  }

  get title () {
    if (this.hasFilters()) {
      var entity = this.store.state.paper.entity
      var filter = entity.getSubEntityByRel(this.rel)
      return filter.properties.title
    }
  }

  get properties () {
    if (this.hasFilters()) {
      var entity = this.store.state.paper.entity
      var filter = entity.getSubEntityByRel(this.rel)
      return filter.properties
    }
  }

  get widgets () {
    if (this.hasFilters()) {
      var entity = this.store.state.paper.entity
      var filter = entity.getSubEntityByRel(this.rel)
      var widgets = filter.getSubEntitiesByClass('widget')
      return widgets
    }
  }

  get links () {
    if (this.hasFilters()) {
      var entity = this.store.state.paper.entity
      var filter = entity.getSubEntityByRel(this.rel)
      return filter.links
    }
  }

  hasFilters () {
    var entity = this.store.state.paper.entity
    if (entity) {
      return entity.hasSubEntityByRel(this.rel)
    }
    return false
  }

  dynamicComponent (widget) {
    switch (widget.properties.type) {
      case this.type.HIDDEN:
        return PaperHidden
      case this.type.DATETIME:
        return PaperDatetime
      case this.type.DATE:
        return PaperDatetime
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
