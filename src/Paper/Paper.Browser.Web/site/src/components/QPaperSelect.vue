<template lang="pug">
  div
    q-select(
      multiple
      chips
      color="secondary"
      :name="widget.properties.name"
      :float-label="widget.properties.title"
      :options="options"
      :label="widget.properties.title"
      v-model="selected"
    )
    input(
      type="hidden"
      :name="field.name"
      :value="selected"
    )
</template>

<script>
export default {
  data: () => ({
    selected: [],
    options: []
  }),

  props: ['widget'],

  created () {
    if (this.field && this.field.value && this.field.value.items) {
      this.options = this.field.value.items
      /*
      this.field.value.items.forEach(item => {
        if (item.selected) {
          this.selected.push(item.value)
        }
      })
      */
    }
  },

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
