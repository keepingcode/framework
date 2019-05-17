<template lang="pug">
  q-field(
    clearable
    class="ma-0 pa-0"
  )
    q-input(
      :float-label="field.title"
      :name="field.name"
      :value="val"
      v-model="field.value"
    )
</template>

<script>
export default {
  props: ['field'],

  computed: {
    val () {
      var selected = this.$paper.browser.selected
      if (selected) {
        var val = this.$paper.browser.selected[this.field.name]
        return val
      }
      return ''
    }
  },

  methods: {
    clear () {
      this.field.value = ''
    }
  }
}
</script>
