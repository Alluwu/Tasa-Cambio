<template>
  <section>
    <TasaCard
      :fecha="tasaDia?.fecha"
      :tasa="tasaDia?.tasa"
      :message="tasaDia?.message"
    />
    <button @click="cargarTasaDia" :disabled="tasaDiaLoading">
      {{ tasaDiaLoading ? 'Cargando...' : 'Actualizar' }}
    </button>
  </section>
</template>

<script setup>
import { onMounted } from 'vue';
import TasaCard from '@/components/TasaCard.vue';
import { useTasaCambioComposable } from '@/composables/useTasaCambioComposable';

const { tasaDia, tasaDiaLoading, cargarTasaDia } = useTasaCambioComposable();

onMounted(() => {
  if (!tasaDia.value) {
    cargarTasaDia();
  }
});
</script>
