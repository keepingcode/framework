<template lang="pug">
  div
    div(
      v-if="hasMenu"
      class="grid-container"
    )
      div(class="menu")
        paper-menu
      div(class="content")
        component(:is='dynamicComponent')

    div(v-else)
      component(:is='dynamicComponent')
</template>

<script>
import PaperGrid from './Grid.vue'
import PaperView from './View.vue'
import PaperForm from './Form.vue'
import PaperMenu from '../components/QPaperMenu.vue'

export default {
  components: {
    PaperGrid,
    PaperView,
    PaperForm,
    PaperMenu
  },

  created () {
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
        case this.$paper.browser.pageTypeEnum.FORM:
          return PaperForm
        default:
          return PaperView
      }
    },

    hasMenu () {
      var hasMenu = this.$paper.data.hasMenu()
      return hasMenu
    }
  }
}
</script>

<style>
.menu { grid-area: menu; }
.content { grid-area: main; }

.grid-container {
  display: grid;
  grid-template-areas:
    'menu main'
    'menu main'
    'menu main';
  width: 100%;
  grid-gap: 5px;
  padding: 5px;
  grid-template-columns: fit-content(40%);
}

.content {
  padding: 10px;
}

.menu {
  width: 250px;
}
</style>
