<template lang="pug">
  q-layout(view="lHh Lpr lFf")
    q-header(elevated)
      q-toolbar
        q-btn(
          flat
          round
          dense
          v-show="showFiltersDrawer"
          @click="openFiltersDrawer()"
        )
          q-icon(name="menu")

        q-toolbar-title
          | Prototipaper

      //-
        q-btn(
          flat
          round
          dense
          v-if="showLinksNavigation"
          @click="openLinksDrawerNavigation()"
        )
          q-icon(name="menu")

    paper-filters-navigation(ref="filtersDrawer")
    paper-drawer-navigation(ref="drawer")

    q-page-container
      router-view
</template>

<script>
import PaperDrawerNavigation from '../components/QPaperDrawerNavigation'
import PaperFiltersNavigation from '../components/QPaperFiltersNavigation'
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

    showFiltersDrawer () {
      var hasFilters = this.$paper.filter.hasFilters()
      return hasFilters
    }
  },

  methods: {
    openLinksDrawerNavigation () {
      this.$refs.filters.drawer = !this.$refs.filters.drawer
    },

    openFiltersDrawer () {
      this.$refs.filtersDrawer.drawer = !this.$refs.filtersDrawer.drawer
    }
  }
}
</script>

<style>
</style>
