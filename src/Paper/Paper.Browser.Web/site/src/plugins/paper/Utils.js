import DataType from './DataType'
import { date } from 'quasar'

export default class Utils {
  constructor (filters) {
    this.filters = filters
    this.dataType = new DataType()
  }

  truncate (text, maxLength) {
    if (text && text.length > maxLength) {
      text = text.substr(0, maxLength - 3) + '...'
    }
    return text
  }

  isString (item) {
    if (item && item.properties) {
      var isString = item.properties.dataType === this.dataType.STRING ||
        item.properties.dataType === this.dataType.TEXT
      return isString
    }
    return false
  }

  format (item, value, truncate) {
    if (item) {
      switch (item.properties.dataType) {
        case this.dataType.TEXT:
        case this.dataType.STRING:
          value = truncate ? this.truncate(value, 30) : value
          break
        case this.dataType.BOOL:
          value = value === 1 ? '\u2714'.normalize() : '\u2717'.normalize()
          break
        case this.dataType.DECIMAL:
          value = this.filters.currency(value, 'R$ ', 2, { thousandsSeparator: '.', decimalSeparator: ',' })
          break
        case this.dataType.DATETIME:
          value = date.formatDate(value, 'DD/MM/YYYY - HH:mm:ss')
          break
      }
    }
    return value
  }
}
