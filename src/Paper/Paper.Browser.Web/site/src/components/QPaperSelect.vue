<template lang="pug">
  div
    q-select(
      color="secondary"
      option-value="key"
      option-label="value"
      :name="widget.properties.name"
      :float-label="widget.properties.title"
      :options="widget.properties.options"
      :label="widget.properties.title"
      v-model="selected"
      emit-value
      map-options
    )
</template>

<script>
export default {
  data: () => ({
    selected: [],
    options: []
  }),

  props: ['widget'],

  methods: {
    clear () {
      this.selected = []
    }
  }
}
</script>

<style lang="stylus">
.label
  margin-left: 8px
</style>
