<template lang="pug">
  div(id="q-app")
    router-view(:key="$route.fullPath")
</template>

<script>
export default {
}
</script>

<style>
</style>
