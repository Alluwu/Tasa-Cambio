<template>
  <section class="tc-layout">
    <!-- Header / acciones -->
    <header class="card tc-header">
      <div>
        <h1>Tipo de Cambio</h1>
      
      </div>

      <div class="tc-actions">
        <button
          class="btn ghost"
          type="button"
          :disabled="tasaDiaLoading"
          @click="abrirTasaDia"
        >
          {{ tasaDiaLoading ? 'Consultando...' : 'Tasa del día' }}
        </button>

        <button
          class="btn primary"
          type="button"
          @click="showRangoModal = true"
        >
          Consulta por rango
        </button>
      </div>
    </header>

    <!-- Filtros + bitácora -->
    <section class="card">
      <form class="tc-filters" @submit.prevent="buscarBitacora">
        <div class="field">
          <label>Fecha inicio</label>
          <input v-model="bitacora.fechaInicio" type="date" />
        </div>
        <div class="field">
          <label>Fecha fin</label>
          <input v-model="bitacora.fechaFin" type="date" />
        </div>
        <div class="field small">
          <label>Tamaño página</label>
          <input v-model.number="bitacora.pageSize" type="number" min="1" max="100" />
        </div>
        <button class="btn primary" type="submit" :disabled="bitacoraLoading">
          {{ bitacoraLoading ? 'Buscando...' : 'Buscar' }}
        </button>
      </form>

      <BitacoraTable :items="bitacora.items" />

      <Pagination
        :page="bitacora.page"
        :total-pages="bitacora.totalPages"
        @update:page="cambiarPagina"
      />

      <p v-if="message" class="tc-message">{{ message }}</p>
    </section>

    <!-- Modal Tasa del día -->
    <BaseModal v-model="showDiaModal" title="Tasa de cambio del día">
      <TasaCard
        :fecha="tasaDia?.fecha"
        :tasa="tasaDia?.tasa"
        :message="tasaDiaMessage"
      />
    </BaseModal>

    <!-- Modal Rango -->
    <BaseModal v-model="showRangoModal" title="Consulta por rango">
      <RangoForm v-model="rangoRequest" @submit="onSubmitRango" />

      <div v-if="rangoLoading" class="tc-loading">
        Consultando rango...
      </div>

      <div v-else-if="rangoResultado" class="tc-rango-result">
        <p class="tc-rango-resumen">
          {{ rangoResultado.message }}
        </p>
        <p class="tc-rango-resumen">
          Total ítems: <strong>{{ rangoResultado.totalItems }}</strong> ·
          Promedio venta: <strong>{{ rangoResultado.promedioVenta }}</strong>
        </p>

        <table class="table">
          <thead>
            <tr>
              <th>Fecha</th>
              <th>Moneda</th>
              <th>Venta</th>
              <th>Compra</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(item, i) in rangoResultado.items" :key="i">
              <td>{{ item.fecha }}</td>
              <td>{{ item.moneda }}</td>
              <td>{{ item.venta }}</td>
              <td>{{ item.compra }}</td>
            </tr>
          </tbody>
        </table>
      </div>

      <template #footer>
        <button class="btn ghost" type="button" @click="showRangoModal = false">
          Cerrar
        </button>
      </template>
    </BaseModal>
  </section>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import BitacoraTable from '@/components/BitacoraTable.vue';
import Pagination from '@/components/Pagination.vue';
import RangoForm from '@/components/RangoForm.vue';
import TasaCard from '@/components/TasaCard.vue';
import BaseModal from '@/components/BaseModal.vue';
import { useTasaCambioComposable } from '@/composables/useTasaCambioComposable';

const showDiaModal = ref(false);
const showRangoModal = ref(false);

const {
  bitacora,
  bitacoraLoading,
  message,
  cargarBitacora,
  setBitacoraFiltros,
  setBitacoraPage,
  tasaDia,
  tasaDiaLoading,
  rangoRequest,
  rangoResultado,
  rangoLoading,
  cargarTasaDia,
  cargarTasaRango,
} = useTasaCambioComposable();

const tasaDiaMessage = computed(() => tasaDia.value?.message || '');

async function abrirTasaDia() {
  showDiaModal.value = true;
  await cargarTasaDia();
  await cargarBitacora();
}

async function onSubmitRango() {
  await cargarTasaRango();
  await cargarBitacora();
}

function buscarBitacora() {
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
