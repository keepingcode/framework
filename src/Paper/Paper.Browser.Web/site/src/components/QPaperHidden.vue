<template lang="pug">
  q-field(
    :label="widget.properties.title"
    :value="widget.properties.value"
    :name="widget.properties.name"
    v-model="widget.properties.value"
    clearable
    class="ma-0 pa-0"
    v-show="false"
  )
</template>

<script>
export default {
  props: ['widget']
}
</script>
