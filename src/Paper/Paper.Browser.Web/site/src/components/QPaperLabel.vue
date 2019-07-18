<template lang="pug">
  label(:class="classLink") {{ label }}
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
    },
    link: {
      type: Boolean,
      default: false
    }
  },

  computed: {
    label () {
      var item = this.$paper.data.headers.getHeader(this.name)
      if (this.isKey) {
        return item.title
      }
      var property = this.$paper.utils.format(item, this.value, this.truncate)
      return property
    },

    classLink () {
      var classLink = this.link ? 'link' : ''
      return classLink
    }
  }
}
</script>

<style lang="stylus">
.link
  cursor: pointer;
</style>
