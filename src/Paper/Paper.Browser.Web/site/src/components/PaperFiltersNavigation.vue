<template lang="pug">
  q-layout-drawer(
    side="left"
    v-model="drawer"
    v-if="showFilters"
    :content-class="$q.theme === 'mat' ? 'bg-grey-2' : null"
  )
    q-scroll-area(class="fit")
      div(style="padding: 25px 16px 16px;")
        paper-form(:action="filters")
</template>

<script>
import PaperForm from '../components/PaperForm'

export default {
  data: () => ({
    drawer: true
  }),

  components: {
    PaperForm
  },

  computed: {
    filters () {
      var filters = this.$paper.form.getFilters()
      return filters
    },

    showFilters () {
      var hasFilters = this.$paper.form.hasFilters()
      return hasFilters
    }
  },

  methods: {
    openUrl (link) {
      this.$paper.browser.openUrl(link)
    }
  }
}
</script>
