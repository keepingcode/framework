<template lang="pug">
  div
    q-btn {{ widget.properties.value }}
</template>

<script>
  export default {
    props: ['widget']
  }
</script>
