<template lang="pug">
  q-input(
    type="number"
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
  }
}
</script>
