<template>
  <section>
    <form @submit.prevent="buscar" class="card form-grid">
      <div>
        <label>Fecha inicio</label>
        <input v-model="bitacora.fechaInicio" type="date" />
      </div>
      <div>
        <label>Fecha fin</label>
        <input v-model="bitacora.fechaFin" type="date" />
      </div>
      <div>
        <label>Tamaño página</label>
        <input v-model.number="bitacora.pageSize" type="number" min="1" max="100" />
      </div>
      <button type="submit" :disabled="bitacoraLoading">
        {{ bitacoraLoading ? 'Buscando...' : 'Buscar' }}
      </button>
    </form>

    <BitacoraTable :items="bitacora.items" />

    <Pagination
      :page="bitacora.page"
      :total-pages="bitacora.totalPages"
      @update:page="cambiarPagina"
    />

    <div class="card" v-if="message">
      {{ message }}
    </div>
  </section>
</template>

<script setup>
import { onMounted } from 'vue';
import BitacoraTable from '@/components/BitacoraTable.vue';
import Pagination from '@/components/Pagination.vue';
import { useTasaCambioComposable } from '@/composables/useTasaCambioComposable';

const {
  bitacora,
  bitacoraLoading,
  message,
  cargarBitacora,
  setBitacoraFiltros,
  setBitacoraPage,
} = useTasaCambioComposable();

function buscar() {
  setBitacoraFiltros({
    fechaInicio: bitacora.value.fechaInicio,
    fechaFin: bitacora.value.fechaFin,
    pageSize: bitacora.value.pageSize,
  });
  cargarBitacora();
}

function cambiarPagina(page) {
  setBitacoraPage(page);
  cargarBitacora();
}

onMounted(cargarBitacora);
</script>
