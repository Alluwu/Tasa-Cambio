<template>
  <section>
    <RangoForm v-model="rangoRequest" @submit="onSubmit" />

    <div v-if="rangoLoading" class="card">Cargando...</div>

    <div v-else-if="rangoResultado" class="card">
      <p>{{ rangoResultado.message }}</p>
      <p>Total ítems: {{ rangoResultado.totalItems }}</p>
      <p>Promedio venta: {{ rangoResultado.promedioVenta }}</p>

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

    <div v-else class="card">
      Ingresa un rango y moneda para consultar.
    </div>
  </section>
</template>

<script setup>
import RangoForm from '@/components/RangoForm.vue';
import { useTasaCambioComposable } from '@/composables/useTasaCambioComposable';

const { rangoRequest, rangoResultado, rangoLoading, cargarTasaRango } = useTasaCambioComposable();

function onSubmit() {
  cargarTasaRango();
}
</script>
