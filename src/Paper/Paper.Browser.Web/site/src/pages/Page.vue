<template lang="pug">
  component(:is='dynamicComponent')
</template>

<script>
import PaperGrid from './Grid.vue'
import PaperView from './View.vue'
import PaperForm from './Form.vue'

export default {
  components: {
    PaperGrid,
    PaperView,
    PaperForm
  },

  props: {
    demonstrationMode: {
      type: Boolean,
      default: false
    }
  },

  created () {
    this.$paper.browser.setDemonstrationMode(this.demonstrationMode)
    this.$paper.browser.load()
  },

  computed: {
    dynamicComponent () {
      // Verifica se deve exibir um formulário
      if (this.$paper.browser.isFormMode()) {
        return PaperForm
      }
      switch (this.$paper.browser.type) {
        case this.$paper.browser.pageTypeEnum.GRID:
          return PaperGrid
        default:
          return PaperView
      }
    }
  }
}
</script>
