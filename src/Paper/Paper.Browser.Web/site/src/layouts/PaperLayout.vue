<template lang="pug">
  q-layout(view="lHh Lpr lFf")
    q-header(elevated)
      q-toolbar
        q-btn(
          flat
          round
          dense
          v-if="showFiltersDrawer"
          @click="openFiltersDrawer()"
        )
          q-icon(name="menu")

        q-toolbar-title
          | {{ $paper.title }}

        q-btn(
          flat
          round
          dense
          v-if="showLinksNavigation"
          @click="openLinksDrawer()"
        )
          q-icon(name="menu")

    paper-filters-navigation(ref="filtersDrawer")
    paper-drawer-navigation(ref="linksDrawer")

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
      return hasLinks
    },

    showFiltersDrawer () {
      var hasFilters = this.$paper.filter.hasFilters()
      return hasFilters
    }
  },

  methods: {
    openLinksDrawer () {
      this.$refs.linksDrawer.drawer = !this.$refs.linksDrawer.drawer
    },

    openFiltersDrawer () {
      this.$refs.filtersDrawer.drawer = !this.$refs.filtersDrawer.drawer
    }
  }
}
</script>

<style>
</style>
