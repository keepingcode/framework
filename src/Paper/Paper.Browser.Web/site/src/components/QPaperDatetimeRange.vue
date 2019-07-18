<template lang="pug">
  q-field(
    :label="field.title"
    clearable
    class="ma-0 pa-0"
  )
    q-input(
      v-model="widget.properties.value"
      :value="widget.properties.value"
      :name="widget.properties.name"
    )
</template>

<script>
export default {
  props: ['widget']
}
</script>
