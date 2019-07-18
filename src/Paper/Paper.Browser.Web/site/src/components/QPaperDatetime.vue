<template lang="pug">
  q-input(
    v-model="date"
    :name="widget.properties.name"
    :label="widget.properties.title"
  )
    template(v-slot:prepend)
      q-icon(
        name="event"
        class="cursor-pointer"
      )
        q-popup-proxy(
          vtransition-show="scale"
          transition-hide="scale"
        )
          q-date(
            v-model="date"
            mask="YYYY-MM-DD HH:mm"
          )

    template(v-slot:append)
      q-icon(
        name="access_time"
        class="cursor-pointer"
      )
        q-popup-proxy(
          transition-show="scale"
          transition-hide="scale"
        )
          q-time(
            v-model="date"
            mask="YYYY-MM-DD HH:mm"
            format24h
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
