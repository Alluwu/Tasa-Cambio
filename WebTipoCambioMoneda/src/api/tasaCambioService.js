import api from './http';

export async function apiGetTasaDia() {
  const res = await api.get('/TasaCambioMoneda');
  return res.data;
}

export async function apiGetTasaRango(params) {
  const res = await api.get('/TasaCambioMoneda/rango', { params });
  return res.data;
}

export async function apiGetBitacora(params) {
  const res = await api.get('/TasaCambioMoneda/bitacora', { params });
  return res.data;
}
