import Data from './paper/Data.js'
import Browser from './paper/Browser.js'
import Utils from './paper/Utils.js'
import Form from './paper/Form.js'
import Filter from './paper/Filter.js'

export default ({ app, router, store, Vue }) => {
  var data = new Data(store)
  var browser = new Browser(store, router)
  var utils = new Utils(Vue.options.filters)
  var form = new Form(Vue.options, router, store, Vue.prototype)
  var filter = new Filter(Vue.options, router, store, Vue.prototype)
  var paper = {
    properties: data.properties,
    data: data,
    browser: browser,
    utils: utils,
    form: form,
    filter: filter,

    get title () {
      var entity = store.state.paper.entity
      if (entity && entity.title) {
        return entity.title
      }
      return 'Paper'
    }
  }
  Vue.prototype.$paper = paper
}
