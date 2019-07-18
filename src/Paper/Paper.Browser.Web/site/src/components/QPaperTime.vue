<template lang="pug">
  q-input(
    filled
    mask="time"
    v-model="date"
    :name="widget.properties.name"
    :label="widget.properties.title"
  )
    template(v-slot:prepend)
      q-icon(
        name="access_time"
        class="cursor-pointer"
      )
        q-popup-proxy(
          vtransition-show="scale"
          transition-hide="scale"
        )
          q-time(
            v-model="time"
          )
</template>

<script>
export default {
  data: () => ({
    time: '10:56',
    timeWithSeconds: '10:56:00'
  }),

  props: ['widget']
}
</script>
