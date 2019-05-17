<template lang="pug">
  q-field(
    :label="field.title"
    :value="field.value"
    :name="field.name"
    v-model="field.value"
    clearable
    class="ma-0 pa-0"
    v-show="false"
  )
</template>

<script>
export default {
  props: ['field']
}
</script>
