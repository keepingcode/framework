<template lang="pug">
  q-field(
    :label="field.title"
    clearable
    class="ma-0 pa-0"
  )
    q-input(
      v-model="field.value"
      :value="field.value"
      :name="field.name"
    )
</template>

<script>
export default {
  props: ['field']
}
</script>
