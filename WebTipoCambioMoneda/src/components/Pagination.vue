<template>
  <div class="pagination">
    <button :disabled="page <= 1" @click="$emit('update:page', page - 1)">
      Anterior
    </button>
    <span>Página {{ page }} de {{ totalPages }}</span>
    <button :disabled="page >= totalPages" @click="$emit('update:page', page + 1)">
      Siguiente
    </button>
  </div>
</template>

<script setup>
const props = defineProps({
  page: { type: Number, default: 1 },
  totalPages: { type: Number, default: 1 },
});
</script>
