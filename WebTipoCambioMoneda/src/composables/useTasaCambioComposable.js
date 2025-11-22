import { storeToRefs } from 'pinia';
import { useTasaCambioStore } from '@/store/useTasaCambioStore';

export const useTasaCambioComposable = () => {
  const store = useTasaCambioStore();

  const {
    tasaDia,
    tasaDiaLoading,
    rangoRequest,
    rangoResultado,
    rangoLoading,
    bitacora,
    bitacoraLoading,
    message,
  } = storeToRefs(store);

  const cargarTasaDia = async () => await store.cargarTasaDia();
  const cargarTasaRango = async () => await store.cargarTasaRango();
  const cargarBitacora = async () => await store.cargarBitacora();

  const setBitacoraFiltros = (payload) => store.setBitacoraFiltros(payload);
  const setBitacoraPage = (page) => store.setBitacoraPage(page);

  return {
    tasaDia,
    tasaDiaLoading,
    rangoRequest,
    rangoResultado,
    rangoLoading,
    bitacora,
    bitacoraLoading,
    message,

    cargarTasaDia,
    cargarTasaRango,
    cargarBitacora,
    setBitacoraFiltros,
    setBitacoraPage,
  };
};
