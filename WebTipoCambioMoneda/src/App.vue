<template>
  <div class="app-root">
    <main class="app-main">
      <section class="app-shell card">
        <header class="app-header">
          <h1 class="app-title">Tipo de Cambio</h1>
          <p class="app-subtitle">
          </p>
        </header>

        <RouterView />
      </section>
    </main>
  </div>
</template>

<script setup>
import { RouterView } from 'vue-router';
</script>
