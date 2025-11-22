<template>
  <teleport to="body">
    <div
      v-if="modelValue"
      class="modal-backdrop"
      @click.self="$emit('update:modelValue', false)"
    >
      <div class="modal-card">
        <header class="modal-header">
          <h3 class="modal-title">{{ title }}</h3>
          <button class="icon-button" type="button" @click="$emit('update:modelValue', false)">
            ×
          </button>
        </header>

        <section class="modal-body">
          <slot />
        </section>

        <footer v-if="$slots.footer" class="modal-footer">
          <slot name="footer" />
        </footer>
      </div>
    </div>
  </teleport>
</template>

<script setup>
defineProps({
  modelValue: { type: Boolean, default: false },
  title: { type: String, default: '' },
});
defineEmits(['update:modelValue']);
</script>
