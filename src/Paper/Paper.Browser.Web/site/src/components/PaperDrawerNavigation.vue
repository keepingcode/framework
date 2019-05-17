<template lang="pug">
  q-layout-drawer(
    side="right"
    v-model="drawer"
    :content-class="$q.theme === 'mat' ? 'bg-grey-2' : null"
  )
    q-list(v-if="showLinks")
      q-list-header
        | NAVEGAÇÃO
      q-item(
        v-for="link in links"
        :key="link.href"
        link
        @click.native="openUrl(link.href)"
      )
        q-item-main(
          :label="link.title"
        )

    q-list(v-if="showActions")
      q-list-header
        | AÇÕES
      q-item(
        v-for="action in actions"
        :key="action.href"
        link
        @click.native="openAction(action.href)"
      )
        q-item-main(
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
