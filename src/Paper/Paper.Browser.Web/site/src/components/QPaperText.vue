<template lang="pug">
  q-input(
    :name="widget.properties.name"
    :value="widget.properties.value"
    :label="widget.properties.title"
    v-model="widget.properties.value"
  )
</template>

<script>
export default {
  props: ['widget'],

  methods: {
    clear () {
      this.widget.value = ''
    }
  }
}
</script>
