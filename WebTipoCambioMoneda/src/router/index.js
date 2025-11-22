import { createRouter, createWebHistory } from 'vue-router';
import TasaCambioDiaView from '@/views/TasaCambioDiaView.vue';
import TasaCambioRangoView from '@/views/TasaCambioRangoView.vue';
import BitacoraView from '@/views/BitacoraView.vue';

const routes = [
  { path: '/', redirect: '/tasa-dia' },
  { path: '/tasa-dia', component: TasaCambioDiaView },
  { path: '/tasa-rango', component: TasaCambioRangoView },
  { path: '/bitacora', component: BitacoraView },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
