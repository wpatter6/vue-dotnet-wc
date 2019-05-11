<template>
  <div id="app">
    <Home msg="Welcome to Your Vue.js + TypeScript App"/>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'
import Home from '@/views/Home.vue'

export default Vue.extend({
  name: 'app',
  components: {
    Home,
  },
})
</script>
