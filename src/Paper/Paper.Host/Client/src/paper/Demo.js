export default class Demo {
  constructor (options, parser, entity) {
    this.router = options.router
    this.store = options.store
    this.parser = parser
    this.vue = options.vm
    this.blueprintPage = '/page/Blueprint'
    this.entity = entity
  }

  isRoot () {
    return location.hash === '#/demo'
  }

  isDemoPage (path) {
    return path &&
            (path.match(/\/page\/demo/g) !== null ||
              path.match(/demo/g) !== null ||
                path.match(/\/demo/g) !== null)
  }

  loadBlueprint () {
    this._importDemoFile(this.blueprintPage).then(json => {
      var data = this.parser.parse(json)
      this.store.commit('blueprint/setBlueprint', data)
    }).catch(error => {
      console.log('erro', error)
      this.vue.$notify({ message: 'Erro ao carregar o blueprint: ' + this.blueprintPage, type: 'danger' })
    })
  }

  load (jsonFile) {
    this.entity.setPath(jsonFile)
    jsonFile = this._makeJsonFilePath(jsonFile)
    this._importDemoFile(jsonFile).then(json => {
      var data = this.parser.parse(json)
      this.entity.setEntity(data)
    }).catch(() => {
      var message = 'Erro ao carregar a página de demonstração: ' + jsonFile
      this.vue.$notify({ message: message, type: 'danger' })
      this.router.replace({name: 'notFound', params: { routerName: jsonFile }})
    })
  }

  _importDemoFile (jsonFile) {
    return import(`../../static/demo${jsonFile}.json`)
  }

  _makeJsonFilePath (jsonFile) {
    if (jsonFile.startsWith(location.origin)) {
      jsonFile = jsonFile.replace(location.origin, '')
    }
    if (!jsonFile.startsWith('/page')) {
      if (!jsonFile.startsWith('/')) {
        jsonFile = '/' + jsonFile
      }
      jsonFile = '/page' + jsonFile
    }
    return jsonFile
  }
}
