<template lang="pug">
  div
    br
    input(
      type="checkbox"
      color="secondary"
      v-model="field.value"
      :value="field.value"
      :name="field.name"
    )
    span(class="label") {{ field.title }}
    br
</template>

<script>
export default {
  props: ['field']
}
</script>

<style lang="stylus">
.label
  margin-left: 8px
</style>
