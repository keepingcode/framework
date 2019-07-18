export default class Data {
  constructor (store) {
    this.store = store
    this.headers = new Headers(this.store)
    this.records = new Records(this.store)
  }

  get self () {
    return this.store.state.paper.entity
  }

  get properties () {
    if (this.self && this.self.properties) {
      return this.self.properties
    }
  }

  get menu () {
    if (this.self && this.self.hasLinkByRel('menu')) {
      var links = this.self.getLinksByRel('menu')
      return links
    }
  }

  hasMenu () {
    if (this.self) {
      return this.self.hasLinkByRel('menu')
    }
    return false
  }

  getProperty (property) {
    if (this.self) {
      var exists = this.self.hasProperty(property)
      if (exists) {
        return this.properties[property]
      }
    }
  }
}

class Headers {
  constructor (store) {
    this.store = store
  }

  get entity () {
    return this.store.state.paper.entity
  }

  get properties () {
    if (this.entity && this.entity.properties) {
      return this.entity.properties.__headers.record
    }
    return []
  }

  getHeader (name) {
    var headers = this.entity.getSubEntitiesByClass('header')
    var item = headers.find(header => header.properties.name === name)
    return item
  }
}

class Records {
  constructor (store) {
    this.store = store
    this.record = new Record()
  }

  get entity () {
    return this.store.state.paper.entity
  }

  get entities () {
    if (this.entity && this.entity.hasSubEntityByClass('record')) {
      var records = this.entity.getSubEntitiesByClass('record')
      return records
    }
  }

  get properties () {
    if (this.entities) {
      return this.entities.map(record => record.properties)
    }
  }

  get headers () {
    if (this.entity && this.entity.hasSubEntityByClass('header')) {
      var records = this.entity.getSubEntitiesByClass('header')
      return records
    }
  }

  getRecord (index) {
    var object = this.entities[index]
    var record = new Record(object)
    return record
  }
}

class Record {
  constructor (record) {
    this.record = record
  }

  get links () {
    if (this.hasLinks) {
      return this.record.links
    }
  }

  get hasLinks () {
    var hasLinks = this.record && this.record.links && this.record.links.length > 0
    return hasLinks
  }

  get selfLink () {
    if (this.hasSelfLink) {
      return this.record.getLinksByRel('self')
    }
  }

  get hasSelfLink () {
    var hasSelfLink = this.record && this.record.hasLinkByRel('self')
    return hasSelfLink
  }
}
