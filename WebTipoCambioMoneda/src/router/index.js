import { createRouter, createWebHistory } from 'vue-router'

import TipoCambioView from '@/views/TipoCambioView.vue'



const routes = [
  { 
    path: '/', 
    name: 'home',
    component: TipoCambioView 
  },

]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

export default router