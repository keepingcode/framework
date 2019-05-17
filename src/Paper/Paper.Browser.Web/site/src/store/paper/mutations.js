import SirenParse from 'siren-parser'

export const parseSiren = (state, data) => {
  var siren = SirenParse(data)
  state.entity = siren
  var hasRecords = siren.hasSubEntityByRel('record')
  if (hasRecords) {
    state.entities = state.entity.getSubEntitiesByRel('record')
  }
}

export const setSelected = (state, data) => {
  state.selected = data
}

export const setDemonstrationMode = (state, data) => {
  state.demonstrationMode = data
}
