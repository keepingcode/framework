import Record from './paper/Record.js'
import Browser from './paper/Browser.js'
import Utils from './paper/Utils.js'
import Form from './paper/Form.js'

export default ({ app, router, store, Vue }) => {
  var record = new Record(store)
  var browser = new Browser(store, router)
  var utils = new Utils(Vue.options.filters)
  var form = new Form(Vue.options, router, store, Vue.prototype)
  var paper = {
    record: record,
    browser: browser,
    utils: utils,
    form: form,

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
