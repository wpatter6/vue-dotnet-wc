<template>
  <div class="home">
    <h1>Home</h1>
    <HelloWorld :msg="msg"></HelloWorld>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'
import HelloWorld from '@/components/HelloWorld.vue'

export default Vue.extend({
  props: {
    msg: { type: String, default: 'Hello World!' },
  },
  components: {
    HelloWorld,
  },
})
</script>

<style lang="scss" scoped>
</style>
