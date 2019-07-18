import axios from 'axios'
import { openURL } from 'quasar'

export default class Requester {
  constructor (store, router) {
    this.store = store
    this.router = router
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
    return axios.request({
      url: href,
      method: method,
      data: params,
      params: getParams,
      headers: header
    }).then(response => {
      return {
        ok: true,
        data: response
      }
    }).catch(error => {
      if (!error.response) {
        return {
          ok: false,
          data: error.response
        }
      }
      console.log('Erro: ', error.response)
      return {
        ok: false,
        data: error.response
      }
    })
  }

  async requestSiren (url, query) {
    return this.httpRequest('get', url, query).then(response => {
      console.log('response', response)
      if (response.ok) {
        try {
          this.store.commit('paper/parseSiren', response.data.data)
          return {
            ok: true,
            data: response.data.data
          }
        } catch (err) { }
      }
      return {
        ok: false
      }
    })
  }

  openUrl (url, params) {
    if (url) {
      var isExternal = this.isExternalUrl(url)
      if (isExternal) {
        openURL(url)
        return
      }
      var isAbsolute = this.isAbsoluteUrl(url)
      if (isAbsolute) {
        var existsParams = params && Object.keys(params).length > 0
        if (existsParams) {
          url += url.indexOf('?') > -1 ? '&' : '?'
          url += Object.entries(params).map(([key, val]) => `${key}=${val}`).join('&')
        }
        window.location = url
        return
      }
      this.router.push({ path: url, query: params })
    }
  }

  isAbsoluteUrl (url) {
    var isAbsoluteUrl = require('is-absolute-url')
    return isAbsoluteUrl(url)
  }

  isExternalUrl (url) {
    var isUrlExternal = require('is-url-external')
    return isUrlExternal(url)
  }
}
