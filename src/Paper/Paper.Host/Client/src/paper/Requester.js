import Errors from './Errors.js'

export default class Requester {

  constructor (options, demo, entity) {
    this.router = options.router
    this.axios = options.axios
    this.vue = options.vm
    this.demo = demo
    this.entity = entity
    this.errors = new Errors()
  }

  redirectToPage (link) {
    if (link) {
      if (link.startsWith('http') && !link.startsWith(window.location.origin)) {
        window.open(link, '_blank')
        return
      }
      this.entity.setPath(link)
      if (this.demo.isDemoPage(link)) {
        this.demo.load(link)
      }
      var params = this._makeParams(link)
      this.router.push({ name: 'page', params: { path: params } })
    }
  }

  redirectToForm (link, action) {
    if (link) {
      this.entity.setPath(link)
      if (!(link instanceof Array)) {
        link = this._makeParams(link)
      }
      this.router.push({ name: 'form', params: { path: link }, query: { action: action } })
    }
  }

  httpRequest (method, href, params) {
    var getParams = method.toLowerCase() === 'get' ? params : ''
    var header = {
      'Accept': 'application/json;application/vnd.siren+json;charset=UTF-8;',
      'Access-Control-Expose-Headers': 'Access-Control-*',
      'Access-Control-Allow-Headers': 'Access-Control-*, Origin, X-Requested-With, Content-Type, Accept',
      'Access-Control-Allow-Methods': 'GET, POST, PUT, DELETE, OPTIONS, HEAD',
      'Access-Control-Allow-Origin': '*',
      'Allow': 'GET, POST, PUT, DELETE, OPTIONS, HEAD'
    }
    return this.axios.request({
      url: href,
      method: method,
      data: params,
      params: getParams,
      headers: header
    }).then(response => {
      return {
        ok: true,
        data: response,
        message: 'Operação realizada com sucesso'
      }
    }).catch(error => {
      var message = 'Erro ao acessar a url'
      if (!error.response) {
        this.vue.$notify({ message: message, type: 'danger' })
        return {
          ok: false,
          data: {}
        }
      }
      console.log('Erro: ', error.response)
      message = message + ': ' + href + '. ' + this.errors.httpTranslate(error.response.status)
      // this.vue.$notify({ message: message, type: 'danger' })
      return {
        ok: false,
        data: error.response
      }
    })
  }

  linkIsExternal (link) {
    return link.startsWith('http') && !link.startsWith(window.location.origin)
  }

  isOpenInAnotherPage (link) {
    return (link.type !== undefined && !link.type.match(/json/g)) || this.linkIsExternal(link.href)
  }

  target (link) {
    if (this.isOpenInAnotherPage(link)) {
      return '_blank'
    }
    return '_self'
  }

  goToRootPage () {
    var link = this.entity.selfLink
    if (link) {
      this.redirectToPage(link.href)
    }
  }

  containsHash (path) {
    var startsWithHash = path.startsWith(window.location.origin + '/#')
    var startsWithIndex = path.toLowerCase().startsWith(window.location.origin + '/index')
    return startsWithHash || startsWithIndex
  }

  addHashToUrl (path) {
    path = path.replace('#/', '')
    path = path.replace(window.location.origin, window.location.origin + '/#')
    return path
  }

  isRoot () {
    var isRoot = window.location.hash.toLowerCase() === '#/index.html' || window.location.hash === '#/'
    return isRoot
  }

  _makeParams (path) {
    if (path.indexOf(location.origin) >= 0) {
      path = path.replace(location.origin, '')
    }
    if (path.startsWith('/page')) {
      path = path.substring(6, path.length)
    }
    if (path.startsWith('page')) {
      path = path.substring(5, path.length)
    }
    if (path.match(/form/g)) {
      path = path.replace('form', '')
    }
    var params = path.split('/')
    params = params.filter(function (x) {
      return (x !== (undefined || null || ''))
    })
    return params
  }

}
