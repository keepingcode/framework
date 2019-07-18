<template lang="pug">
  div
    br
    input(
      type="checkbox"
      v-model="widget.properties.value"
      :value="widget.properties.value"
      :name="widget.properties.name"
    )
    span(class="label") {{ widget.properties.title }}
    br
</template>

<script>
export default {
  props: ['widget']
}
</script>

<style lang="stylus">
.label
  margin-left: 8px
</style>
