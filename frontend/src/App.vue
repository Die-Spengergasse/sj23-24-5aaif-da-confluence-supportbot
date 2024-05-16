<script setup>
import { useUserStore } from '@/stores/user'
import { RouterLink, RouterView } from 'vue-router'

const user = useUserStore();
</script>

<template>
  <header>
    <nav>
      <h5 class="navbarHeader">Angemeldet als {{ user.userdata.username }}
            <small>({{ user.userdata.guid }})</small>
          </h5>
    </nav>
  </header>
  <main>
    <RouterView />
  </main>
</template>

<style scoped>

</style>
