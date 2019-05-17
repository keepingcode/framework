<template lang="pug">
  q-modal(
    v-model="opened"
    :content-css="{ minWidth: '80vw', minHeight: '80vh' }"
  )
    q-modal-layout
      q-toolbar(slot="header")
        q-btn(
          flat
          round
          dense
          v-close-overlay
          icon="keyboard_arrow_left"
        )
        q-toolbar-title
          Header

      div(class="layout-padding")
        h1 {{ action.title }}

        q-btn(
          color="primary"
          v-close-overlay
          label="Close"
        )

        p This is a Modal presenting a Layout.
</template>

<script>
export default {
  data: () => ({
    opened: false
  }),

  computed: {
    action () {
      var action = this.$paper.browser.action
      return action
    }
  }
}
</script>
