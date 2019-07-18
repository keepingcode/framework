<template lang="pug">
  span(v-if="visible")
    q-icon(
      @click.stop=""
      small
      class="mr-2"
      @click="showDialog"
      name="visibility"
      style="cursor: pointer"
    )

    q-paper-dialog(
      :value="text"
      :show.sync="dialogVisible"
      :title="title"
      hide-title
    )
</template>

<script>
import QPaperDialog from './QPaperDialog.vue'
export default {
  props: {
    text: String,
    title: String,
    hideTitle: {
      type: Boolean,
      default: false
    }
  },

  components: {
    QPaperDialog
  },

  data: () => ({
    dialogVisible: false
  }),

  computed: {
    visible () {
      var visible = this.$paper.utils.isTruncate(this.text)
      return visible
    }
  },

  methods: {
    showDialog () {
      this.dialogVisible = true
    }
  }
}
</script>
