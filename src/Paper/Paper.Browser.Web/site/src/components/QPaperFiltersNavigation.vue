<template lang="pug">
  q-drawer(
    side="left"
    v-model="drawer"
    content-class="bg-grey-2"
  )
    q-scroll-area(class="fit")
      div(style="padding: 25px 16px 16px;")
        paper-form(
          :widgets="$paper.filter.widgets"
          :title="$paper.filter.title"
          :links="$paper.filter.links"
        )
</template>

<script>
import PaperForm from '../components/QPaperForm'

export default {
  data: () => ({
    drawer: false
  }),

  components: {
    PaperForm
  }
}
</script>
