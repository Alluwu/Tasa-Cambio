<template>
  <div class="card">
    <table class="table">
      <thead>
        <tr>
          <th>Fecha consulta</th>
          <th>Fecha tipo cambio</th>
          <th>Tasa</th>
          <th>Origen</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="item in items" :key="item.id">
          <td>{{ formatDateTime(item.fechaConsulta) }}</td>
          <td>{{ formatDate(item.fechaTipoCambio) }}</td>
          <td>{{ item.tipoCambio }}</td>
          <td>{{ item.origenApi }}</td>
        </tr>
        <tr v-if="!items || !items.length">
          <td colspan="4">Sin registros</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup>
import { formatDateTime, formatDate } from '@/utils/formatDate';

const props = defineProps({
  items: {
    type: Array,
    default: () => [],
  },
});
</script>
