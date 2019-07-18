<template lang="pug">
  q-input(
    v-model="date"
    mask="date"
    :name="widget.properties.name"
    :label="widget.properties.title"
  )
    template(v-slot:append)
      q-icon(
        name="access_time"
        class="cursor-pointer"
      )
        q-popup-proxy(
          ref="qDateProxy"
          transition-show="scale"
          transition-hide="scale"
        )
          q-date(
            v-model="date"
            @input="() => $refs.qDateProxy.hide()"
          )
</template>

<script>
export default {
  data: () => ({
    date: ''
  }),

  props: ['widget']
}
</script>
