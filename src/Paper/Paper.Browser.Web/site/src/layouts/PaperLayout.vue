<template lang="pug">
  q-layout(view="lHh Lpr lFf")
    q-layout-header
      q-toolbar(color="primary")
        q-btn(
          flat
          round
          dense
          v-if="showFiltersNavigation"
          @click="openFiltersDrawerNavigation()"
          icon="menu"
        )

        q-toolbar-title
          | Prototipaper

      //-
        q-btn(
          flat
          round
          dense
          v-if="showLinksNavigation"
          @click="openLinksDrawerNavigation()"
          icon="menu"
        )

    paper-filters-navigation(
      ref="paperFiltersNavigation"
    )

    paper-drawer-navigation(
      ref="paperLinksNavigation"
    )

    q-page-container
      router-view
</template>

<script>
import PaperDrawerNavigation from '../components/PaperDrawerNavigation'
import PaperFiltersNavigation from '../components/PaperFiltersNavigation'
export default {
  components: {
    PaperDrawerNavigation,
    PaperFiltersNavigation
  },

  computed: {
    showLinksNavigation () {
      var hasLinks = this.$paper.browser.hasLinks()
      var hasActions = this.$paper.browser.hasActions()
      return hasLinks || hasActions
    },

    showFiltersNavigation () {
      // var hasFilters = this.$paper.browser.hasFilters()
      return true
    }
  },

  methods: {
    openLinksDrawerNavigation () {
      this.$refs.paperLinksNavigation.drawer = !this.$refs.paperLinksNavigation.drawer
    },

    openFiltersDrawerNavigation () {
      this.$refs.paperFiltersNavigation.drawer = !this.$refs.paperFiltersNavigation.drawer
    }
  }
}
</script>

<style>
</style>
