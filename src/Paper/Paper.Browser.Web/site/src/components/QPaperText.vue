<template lang="pug">
  q-input(
    :name="widget.properties.name"
    :value="value"
    :label="widget.properties.title"
    v-model="value"
  )
</template>

<script>
export default {
  props: ['widget'],

  computed: {
    value () {
      return ''
    }
  }
}
</script>
