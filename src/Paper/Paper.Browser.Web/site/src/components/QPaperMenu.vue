<template lang="pug">
  div
    q-list(
      dense
      padding
    )
      q-item-label(header) Menu
      q-item(
        v-for="menu in items"
        :key="menu.href"
        clickable
        @click.native="onClick(menu)"
      )
        q-item-section
          q-item-label {{ menu.title }}
</template>

<script>
export default {
  computed: {
    items () {
      var menu = this.$paper.data.menu
      return menu
    }
  },

  methods: {
    onClick (menu) {
      this.$paper.browser.loadPage(menu.href)
        .then(response => {
          if (!response.ok) {
            this.$q.notify('Erro ao carregar o menu.')
          }
        })
    }
  }
}
</script>
