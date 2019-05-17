<template lang="pug">
  q-input(
    :value="field.value"
    :name="field.name"
    :float-label="field.title"
    prefix="R$"
  )
</template>

<script>
import { VMoney } from 'v-money'
export default {
  props: ['field'],

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
