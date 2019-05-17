<template lang="pug">
  div
    q-btn {{ field.value }}
</template>

<script>
  export default {
    props: ['field']
  }
</script>
