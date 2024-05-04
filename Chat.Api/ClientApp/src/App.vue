<script setup lang="ts">
import HelloWorld from './components/HelloWorld.vue'
import Chat from './components/Chat.vue'
</script>

<template>

  <main>
    <div class="wrapper">
      <HelloWorld msg="Welcome to chat!" />
    </div>
    <Chat />
  </main>
</template>

<style scoped>
header {
  line-height: 1.5;
}

</style>
