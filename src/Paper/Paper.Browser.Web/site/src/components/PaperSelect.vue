<template lang="pug">
  div
    q-select(
      multiple
      chips
      color="secondary"
      :name="field.name"
      :float-label="field.title"
      :options="options"
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

  props: ['field'],

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
