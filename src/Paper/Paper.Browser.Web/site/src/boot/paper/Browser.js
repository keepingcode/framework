import PageTypeEnum from './PageType.js'
import Requester from './Requester.js'

export default class Browser {
  constructor (store, router) {
    this.store = store
    this.router = router
    this.pageTypeEnum = PageTypeEnum
    this.requester = new Requester(store, router)
    this.pagination = new Pagination(store, this.requester)
    this.action = new Action(store, this.router, this.requester)
  }

  get isDemonstrationMode () {
    var isDemonstrationMode = this.store.state.paper.demonstrationMode
    return isDemonstrationMode
  }

  get title () {
    var entity = this.store.state.paper.entity
    if (entity) {
      return entity.title
    }
  }

  get type () {
    var entity = this.store.state.paper.entity
    if (entity) {
      if (entity.hasClass('table')) {
        return PageTypeEnum.GRID
      } else if (entity.hasClass('form')) {
        return PageTypeEnum.FORM
      }
    }
    return PageTypeEnum.VIEW
  }

  get links () {
    if (this.hasLinks()) {
      var entity = this.store.state.paper.entity
      var links = entity.getLinksByRel('link')
      return links
    }
  }

  get selfLink () {
    if (this.hasSelfLink()) {
      var entity = this.store.state.paper.entity
      var links = entity.getLinkByRel('self')
      return links
    }
  }

  get actions () {
    var entity = this.store.state.paper.entity
    if (entity && entity.actions) {
      var actions = entity.actions.filter(action => action.name !== '__filter')
      return actions
    }
  }

  get selected () {
    return this.store.state.paper.selected
  }

  hasActions () {
    var hasActions = this.actions && this.actions.length > 0
    return hasActions
  }

  hasLinks () {
    var entity = this.store.state.paper.entity
    if (entity) {
      return entity.hasLinkByRel('link')
    }
  }

  hasSelfLink () {
    var entity = this.store.state.paper.entity
    if (entity) {
      return entity.hasLinkByRel('self')
    }
  }

  openUrl (url) {
    this.requester.openUrl(url)
  }

  isFormMode () {
    var route = this.router.currentRoute.path
    var lastIndex = route.lastIndexOf('/') + 1
    var lastURLSegment = route.substr(lastIndex)
    var isFormMode = lastURLSegment && lastURLSegment.startsWith('-')
    return isFormMode
  }

  setSelected (selected) {
    this.store.commit('paper/setSelected', selected)
  }

  loadDemo () {
    var rootRouteName = this.router.currentRoute.params.routeName.match(/^([^/]+)/)[0]
    var url = `/statics/demo/${rootRouteName}.json`
    if (this.isFormMode()) {
      url = url.substring(0, url.lastIndexOf('/'))
    }
    this.requester.requestSiren(url, this.router.currentRoute.query)
  }

  load () {
    var url = `${this.router.currentRoute.params.path}?f=json+siren`
    if (this.isFormMode()) {
      url = url.substring(0, url.lastIndexOf('/'))
    }
    this.requester.requestSiren(url, this.router.currentRoute.query)
  }

  async sort (sortBy) {
    var entity = this.store.state.paper.entity
    if (entity && entity.hasSubEntityByClass('header')) {
      var headers = entity.getSubEntitiesByClass('header')
      var header = headers.find(header => header.properties.name === sortBy)
      if (header) {
        var sortLink = header.getLinkByRel('sort')
        return this.requester.requestSiren(sortLink.href)
          .then(response => {
            return response
          })
      }
    }
  }

  async loadPage (page) {
    if (this.router.currentRoute.name === 'demo') {
      page = `/statics${page}.json`
    }
    return this.requester.requestSiren(page)
  }
}

class Pagination {
  constructor (store, requester) {
    this.store = store
    this.requester = requester
  }

  get firstPage () {
    if (this.hasFirstPage) {
      var entity = this.store.state.paper.entity
      return entity.getLinkByRel('first')
    }
  }

  get nextPage () {
    if (this.hasNextPage) {
      var entity = this.store.state.paper.entity
      return entity.getLinkByRel('next')
    }
  }

  get previousPage () {
    if (this.hasPreviousPage) {
      var entity = this.store.state.paper.entity
      return entity.getLinkByRel('prev')
    }
  }

  hasNextPage () {
    var entity = this.store.state.paper.entity
    if (entity) {
      return entity.hasLinkByRel('next')
    }
  }

  hasPreviousPage () {
    var entity = this.store.state.paper.entity
    if (entity) {
      return entity.hasLinkByRel('prev')
    }
  }

  hasFirstPage () {
    var entity = this.store.state.paper.entity
    if (entity) {
      return entity.hasLinkByRel('first')
    }
  }

  goToFirstPage () {
    this.requester.requestSiren(this.firstPage.href)
  }

  async loadPage (page) {
    var entity = this.store.state.paper.entity
    if (entity && entity.hasLinkByRel(page)) {
      var link = entity.getLinkByRel(page)
      return this.requester.requestSiren(link.href)
    }
  }

  goToPreviousPage () {
    this.requester.requestSiren(this.previousPage.href)
  }
}

class Action {
  constructor (store, router, requester) {
    this.store = store
    this.router = router
    this.requester = requester
  }

  get current () {
    var entity = this.store.state.paper.entity
    var actionName = this.getName()
    if (entity && entity.hasActionByName(actionName)) {
      var action = entity.getActionByName(actionName)
      return action
    }
  }

  get title () {
    if (this.current) {
      return this.current.title
    }
    return ''
  }

  get fields () {
    if (this.current) {
      return this.current.fields
    }
    return []
  }

  get link () {
    if (this.current) {
      return this.current.href
    }
  }

  get method () {
    if (this.current) {
      return this.current.method
    }
    return 'POST'
  }

  getName () {
    var route = this.router.currentRoute.path
    var lastIndex = route.lastIndexOf('/') + 1
    var actionName = route.substr(lastIndex)
    if (actionName && actionName.startsWith('-')) {
      actionName = actionName.substr(1)
      return actionName.toLowerCase()
    }
  }
}
