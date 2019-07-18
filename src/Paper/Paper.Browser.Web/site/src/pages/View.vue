<template lang="pug">
  div(class="q-pa-lg")
    div(
      xs12
      sm12
      md8
      offset-md2
    )
      q-page(class="self-center")
        q-list(highlight)
          q-item-label(header)
            | {{ title }}
          q-item(
            v-for="item in items"
            :key="item"
          )
            q-item-section
              q-item-label
                q-paper-label(
                  :name="item"
                  isKey
                )
              q-item-label
                q-paper-label(
                  :name="item"
                  :value="getProperty(item)"
                )
</template>

<style>
</style>

<script>
import QPaperLabel from '../components/QPaperLabel.vue'
export default {
  components: {
    QPaperLabel
  },

  computed: {
    title () {
      return this.$paper.title
    },

    items () {
      return this.$paper.data.headers.properties
    }
  },

  methods: {
    getProperty (property) {
      return this.$paper.data.getProperty(property)
    }
  }
}
</script>
