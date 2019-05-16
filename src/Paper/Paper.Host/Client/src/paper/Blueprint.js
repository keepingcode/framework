import staticBlueprint from '../../static/Blueprint.json'
import themes from '../../static/Themes.json'
import Parser from './Parser.js'
export default class Blueprint {

  constructor (options, page, demo, requester) {
    this.store = options.store
    this.page = page
    this.router = options.router
    this.vue = options.vm
    this.requester = requester
    this.blueprintPage = '/Api/1/Blueprint'
    this.blueprint = this.store.getters['blueprint/blueprint']
    this.parser = new Parser(options)
    this.demo = demo
    this.defaultTheme = 'indigo'
  }

  get themes () {
    return themes
  }

  get title () {
    if (this.blueprint && this.blueprint.hasOwnProperty('title')) {
      return this.blueprint.title
    }
    return ''
  }

  get plannerPage () {
    if (this.blueprint && this.blueprint.hasLinkByRel('planner')) {
      return this.blueprint.getLinkByRel('planner').href
    }
    return '#'
  }

  get indexPage () {
    if (this.blueprint && this.blueprint.hasLinkByRel('index')) {
      return this.blueprint.getLinkByRel('index').href
    }
  }

  get projectName () {
    if (this.hasProjectInfo()) {
      return this.blueprint.properties.info.name
    }
    return ''
  }

  get projectDescription () {
    if (this.hasProjectInfo()) {
      return this.blueprint.properties.info.description
    }
    return ''
  }

  get projectVersion () {
    if (this.hasProjectInfo()) {
      return this.blueprint.properties.info.version
    }
    return ''
  }

  get projectInfo () {
    if (this.hasProjectInfo()) {
      return this.blueprint.properties.info
    }
  }

  get blueprintTheme () {
    return this.blueprint.properties.theme
  }

  hasProjectInfo () {
    return this.blueprint && this.blueprint.hasProperty('info')
  }

  hasPlannerPage () {
    return this.blueprint && this.blueprint.hasLinkByRel('planner')
  }

  hasIndexPage () {
    return this.blueprint && this.blueprint.hasLinkByRel('index')
  }

  hasBlueprintTheme () {
    return this.blueprint && this.blueprint.hasProperty('theme')
  }

  showNavBox () {
    if (this.blueprint && this.blueprint.hasProperty('hasNavBox')) {
      return this.blueprint.properties.hasNavBox === 1
    }
    return true
  }

  setBlueprint (blueprint) {
    this.store.commit('blueprint/setEntity', blueprint)
    this.blueprint = this.store.getters['blueprint/blueprint']
  }

  setPredefinedTheme (predefinedTheme) {
    var theme = themes[predefinedTheme]
    this.vue.$vuetify.theme.primary = theme.primary
    this.vue.$vuetify.theme.secondary = theme.secondary
    this.vue.$vuetify.theme.accent = theme.accent
    this.vue.$vuetify.theme.error = theme.error
    this.vue.$vuetify.theme.warning = theme.warning
    this.vue.$vuetify.theme.info = theme.info
    this.vue.$vuetify.theme.success = theme.success
  }

  setTheme () {
    var defaultTheme = themes[this.defaultTheme]
    if (this.hasBlueprintTheme()) {
      var blueprintTheme = themes[this.blueprintTheme]
    }
    var theme = blueprintTheme || defaultTheme
    this.vue.$vuetify.theme.primary = theme.primary
    this.vue.$vuetify.theme.secondary = theme.secondary
    this.vue.$vuetify.theme.accent = theme.accent
    this.vue.$vuetify.theme.error = theme.error
    this.vue.$vuetify.theme.warning = theme.warning
    this.vue.$vuetify.theme.info = theme.info
    this.vue.$vuetify.theme.success = theme.success
  }

  goToIndexPage () {
    var indexPage = this.indexPage
    if (!indexPage) {
      return
    }

    if (this.demo.isDemoPage(indexPage)) {
      this.requester.redirectToPage(indexPage)
      return
    }

    this.page.parse(indexPage).then((response) => {
      if (response && response.ok) {
        this.requester.redirectToPage(indexPage)
      } else {
        this.router.push({ name: 'home' })
      }
    })
  }

  load () {
    return new Promise((resolve) => {
      if (this.blueprint === null) {
        this.page.parse(this.blueprintPage).then(response => {
          if (response && response.ok) {
            this.setBlueprint(response.data)
          } else {
            var blueprint = this.parser.parse(staticBlueprint)
            this.setBlueprint(blueprint)
          }
          this.setTheme()
          resolve()
        })
      } else {
        resolve()
      }
    })
  }
}
