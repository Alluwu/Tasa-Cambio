import { defineStore } from 'pinia';
import { toast } from 'vue3-toastify';
import { apiGetTasaDia, apiGetTasaRango, apiGetBitacora } from '@/api/tasaCambioService';

const defaultBitacoraState = () => ({
  page: 1,
  pageSize: 20,
  totalPages: 0,
  totalItems: 0,
  items: [],
  fechaInicio: '',
  fechaFin: '',
});

export const useTasaCambioStore = defineStore('tasaCambio', {
  state: () => {
    const storedBitacora = localStorage.getItem('bitacoraState');
    const storedTasaDia = localStorage.getItem('tasaDia');

    return {
      tasaDia: storedTasaDia ? JSON.parse(storedTasaDia) : null,
      tasaDiaLoading: false,

      rangoRequest: {
        fechaInicio: '',
        fechaFin: '',
        moneda: 'USD',
      },
      rangoResultado: null,
      rangoLoading: false,

      bitacora: storedBitacora ? JSON.parse(storedBitacora) : defaultBitacoraState(),
      bitacoraLoading: false,
      message: '',
    };
  },

  actions: {
    mostrarMensaje(mensaje, timeout = 2500) {
      toast.success(mensaje, { timeout });
    },

    async cargarTasaDia() {
      this.tasaDiaLoading = true;
      this.message = '';
      try {
        const res = await apiGetTasaDia();
        const payload = res.data ?? res;
        this.tasaDia = payload;
        localStorage.setItem('tasaDia', JSON.stringify(this.tasaDia));

        const msg = res.message ?? payload.message ?? 'Tasa del día actualizada';
        this.mostrarMensaje(msg);
      } catch {
        this.message = 'Error al consultar la tasa del día';
        toast.error(this.message);
      } finally {
        this.tasaDiaLoading = false;
      }
    },

    async cargarTasaRango() {
      this.rangoLoading = true;
      this.message = '';
      try {
        const params = {
          fechaInicio: this.rangoRequest.fechaInicio,
          fechaFin: this.rangoRequest.fechaFin,
          moneda: this.rangoRequest.moneda,
        };

        const res = await apiGetTasaRango(params);
        const payload = res.data ?? res;

        this.rangoResultado = payload;
        const msg = res.message ?? payload.message ?? 'Consulta de rango ejecutada';
        this.mostrarMensaje(msg);
      } catch {
        this.message = 'Error al consultar el rango de tipo de cambio';
        toast.error(this.message);
      } finally {
        this.rangoLoading = false;
      }
    },

    async cargarBitacora() {
      this.bitacoraLoading = true;
      this.message = '';
      try {
        const params = {
          Page: this.bitacora.page,
          PageSize: this.bitacora.pageSize,
          FechaInicio: this.bitacora.fechaInicio || null,
          FechaFin: this.bitacora.fechaFin || null,
        };

        const res = await apiGetBitacora(params);
        const payload = res.data ?? res;

        this.bitacora = {
          ...this.bitacora,
          page: payload.page ?? this.bitacora.page,
          pageSize: payload.pageSize ?? this.bitacora.pageSize,
          totalPages: payload.totalPages ?? 0,
          totalItems: payload.totalItems ?? 0,
          items: payload.items ?? [],
        };

        localStorage.setItem('bitacoraState', JSON.stringify(this.bitacora));
        this.message = payload.message ?? res.message ?? '';
      } catch {
        this.message = 'Error al consultar la bitácora';
        toast.error(this.message);
      } finally {
        this.bitacoraLoading = false;
      }
    },

    setBitacoraFiltros({ fechaInicio, fechaFin, pageSize }) {
      if (fechaInicio !== undefined) this.bitacora.fechaInicio = fechaInicio;
      if (fechaFin !== undefined) this.bitacora.fechaFin = fechaFin;
      if (pageSize !== undefined) this.bitacora.pageSize = pageSize;
      this.bitacora.page = 1;
    },

    setBitacoraPage(page) {
      this.bitacora.page = page;
    },
  },
});
