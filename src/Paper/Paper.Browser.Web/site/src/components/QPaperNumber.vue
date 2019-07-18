<template lang="pug">
  q-input(
    type="number"
    :name="widget.properties.name"
    :value="widget.properties.value"
    :label="widget.properties.title"
    v-model="widget.properties.value"
  )
</template>

<script>
export default {
  props: ['widget']
}
</script>
