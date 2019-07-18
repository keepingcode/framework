<template lang="pug">
  q-dialog(
    v-model="show"
  )
    q-card(style="min-width: 500px; max-width: 80vw;")

      q-card-section(class="row items-center")
        div(
          v-if="title"
          class="text-h6"
        ) {{ title }}

        q-space

        q-btn(
          flat
          round
          dense
          icon="close"
          @click="close"
        )

      q-card-section(
        style="max-height: 1000vh"
        class="modal-container"
      ) {{ value }}
</template>

<script>
export default {
  props: {
    value: String,
    title: String,
    hideTitle: {
      type: Boolean,
      default: false
    },
    show: {
      type: Boolean,
      default: true,
      required: true
    }
  },

  methods: {
    close () {
      this.$emit('update:show', false)
    }
  }
}
</script>

<style lang="stylus">
.modal-container
  display: inline-block;
  width: auto;
</style>
