<template lang="pug">
  q-drawer(
    side="right"
    v-model="drawer"
    bordered
    content-class="bg-grey-2"
  )
    q-list(v-if="showLinks")
      q-item-label(header)
        | NAVEGAÇÃO
      q-item(
        v-for="link in links"
        :key="link.href"
        clickable
        v-ripple
        @click.native="openUrl(link.href)"
      )
        q-item-section
          q-item-label(
            :label="link.title"
          )

    q-list(v-if="showActions")
      q-item-label(header)
        | AÇÕES
      q-item(
        v-for="action in actions"
        :key="action.href"
        v-ripple
        clickable
        @click.native="openAction(action.href)"
      )
        q-item-section
          q-item-label(
            :label="action.title"
          )
</template>

<script>
export default {
  data: () => ({
    drawer: false
  }),

  computed: {
    links () {
      var links = this.$paper.browser.links
      return links
    },

    actions () {
      var actions = this.$paper.browser.actions
      return actions
    },

    showLinks () {
      var hasLinks = this.$paper.browser.hasLinks()
      return hasLinks
    },

    showActions () {
      var hasActions = this.$paper.browser.hasActions()
      return hasActions
    }
  },

  methods: {
    openUrl (link) {
      this.$paper.browser.openUrl(link)
    },

    openAction (link) {
      var selected = this.$paper.browser.selected
      if (selected && selected.length > 0) {
        this.$paper.browser.openUrl(link)
      } else {
        this.$q.dialog({
          title: 'Nenhum registro selecionado',
          message: 'Selecione pelo menos um registro para continuar.'
        })
      }
    }
  }
}
</script>
