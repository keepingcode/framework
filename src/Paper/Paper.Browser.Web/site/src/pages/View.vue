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
          q-list-header
            | {{ title }}
          q-item(
            v-for="item in items"
            :key="item"
          )
            q-item-main
              q-item-tile(label)
                q-paper-label(
                  :name="item"
                  isKey
                )
              q-item-tile(sublabel)
                q-paper-label(
                  :name="item"
                  :value="getProperty(item)"
                )
</template>

<style>
</style>

<script>
import QPaperLabel from '../components/PaperLabel.vue'
export default {
  components: {
    QPaperLabel
  },

  computed: {
    title () {
      return this.$paper.title
    },

    items () {
      return this.$paper.record.headers.properties
    }
  },

  methods: {
    getProperty (property) {
      return this.$paper.record.getProperty(property)
    }
  }
}
</script>
