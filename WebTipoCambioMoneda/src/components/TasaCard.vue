<template>
  <div class="card">
    <h2>Tasa de cambio del día</h2>
    <p v-if="fecha">Fecha: <strong>{{ fecha }}</strong></p>
    <p v-if="tasa">Tasa: <strong>{{ tasa }}</strong></p>
    <p v-if="message" class="msg">{{ message }}</p>
  </div>
</template>

<script setup>
const props = defineProps({
  fecha: String,
  tasa: [String, Number],
  message: String,
});
</script>
