<template lang="pug">
  q-input(
    :value="widget.properties.value"
    :name="widget.properties.name"
    :float-label="widget.properties.title"
    :label="widget.properties.title"
    prefix="R$"
  )
</template>

<script>
import { VMoney } from 'v-money'
export default {
  props: ['widget'],

  data: () => ({
    money: {
      decimal: ',',
      thousands: '.',
      precision: 2,
      masked: false
    }
  }),

  directives: {
    money: VMoney
  }
}
</script>
