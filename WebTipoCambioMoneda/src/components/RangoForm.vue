<template>
  <form @submit.prevent="onSubmit" class="card form-grid">
    <div>
      <label>Fecha inicio</label>
      <input v-model="local.fechaInicio" type="date" required />
    </div>
    <div>
      <label>Fecha fin</label>
      <input v-model="local.fechaFin" type="date" required />
    </div>
    <div>
      <label>Moneda</label>
      <input v-model="local.moneda" type="text" required />
    </div>
    <button type="submit">Consultar</button>
  </form>
</template>

<script setup>
import { reactive, watch } from 'vue';

const props = defineProps({
  modelValue: {
    type: Object,
    default: () => ({ fechaInicio: '', fechaFin: '', moneda: 'USD' }),
  },
});
const emits = defineEmits(['update:modelValue', 'submit']);

const local = reactive({ ...props.modelValue });

watch(
  () => props.modelValue,
  (val) => Object.assign(local, val)
);

watch(
  local,
  (val) => emits('update:modelValue', { ...val }),
  { deep: true }
);

function onSubmit() {
  emits('submit', { ...local });
}
</script>
