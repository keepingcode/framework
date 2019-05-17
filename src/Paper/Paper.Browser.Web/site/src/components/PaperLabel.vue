<template lang="pug">
  label {{ label }}
</template>

<script charset="utf-8">
export default {
  props: {
    name: String,
    value: [String, Number, Date, Array, Object],
    isKey: {
      type: Boolean,
      default: false
    },
    truncate: {
      type: Boolean,
      default: false
    }
  },

  computed: {
    label () {
      var item = this.$paper.record.headers.getHeader(this.name)
      if (this.isKey) {
        return item.title
      }
      var property = this.$paper.utils.format(item, this.value, this.truncate)
      return property
    }
  }
}
</script>
